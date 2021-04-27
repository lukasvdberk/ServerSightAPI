using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerSightAPI.Background;
using ServerSightAPI.DTO;
using ServerSightAPI.DTO.Server;
using ServerSightAPI.EventLoggers;
using ServerSightAPI.Middleware;
using ServerSightAPI.Models;
using ServerSightAPI.Models.Server;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Controllers
{
    [ApiController]
    [Route("api/servers/{serverId:Guid}/cpus")]
    public class CpuUsageServerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IBaseServerEventLogger _baseServerEventLogger;

        public CpuUsageServerController(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IBackgroundJobClient backgroundJobClient,
            IBaseServerEventLogger baseServerEventLogger
        )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _backgroundJobClient = backgroundJobClient;
            _baseServerEventLogger = baseServerEventLogger;
        }
        
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IList<CpuUsageDto>> GetCpuUsageOfServer(Guid serverId, [FromQuery] CreatedBetween createdBetween)
        {
            var server = await ServerUtilController.GetServerFromJwt(serverId, HttpContext);
            
            var cpusUsageServers = await _unitOfWork.CpuUsagesServers.GetAll(
                q => q.ServerId == server.Id 
            );
            
            // year 1 is the default which means the user dit not provide a year.
            if (createdBetween.From.Year != 1 && createdBetween.From.Year != 1)
            {
                cpusUsageServers = await _unitOfWork.CpuUsagesServers.GetAll(
                    q => 
                        server.Id == q.ServerId &&
                        q.CreatedAt >= createdBetween.From && q.CreatedAt <= createdBetween.To
                );
            }
            return _mapper.Map<IList<CpuUsageDto>>(cpusUsageServers);
        }
        
        [HttpPost]
        [ApiKeyAuthorization]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveCpuUsageMinuteOfServer(Guid serverId, [FromBody] CpuUsageDto cpuUsageDto)
        {
            // TODO check if not something was already posted in the past minute
            var server = await ServerUtilController.GetUserHisServerFromApiKey(serverId, HttpContext);

            var cpuUsage = _mapper.Map<CpuUsageServer>(cpuUsageDto);

            cpuUsage.ServerId = server.Id;
            cpuUsage.CreatedAt = DateTime.Now;
            
            await _unitOfWork.CpuUsagesServers.Insert(cpuUsage);

            cpuUsage.Server = server;
            // check if within a minute a new cpu usage was reported. It has 40 seconds to post.
            // This check is to see if the server is offline or not
            _backgroundJobClient.Schedule<ServerPowerStatusSetter>(s => 
                    s.SetServerPowerStatus(server),
            new TimeSpan(0, 1, 10)
            );

            await CPUEventLoggerChecker(cpuUsage);
            
            return NoContent();
        }

        private async Task CPUEventLoggerChecker(CpuUsageServer cpuUsage)
        {
            var notificationThreshold = await _unitOfWork.NotificationThresholds.Get(
                q => q.ServerId == cpuUsage.ServerId
            );
            
            if(notificationThreshold == null) {return;}
            if (notificationThreshold.CpuUsageThresholdInPercentage <= cpuUsage.AverageCpuUsagePastMinute)
            {
                await CPUServerEventLogger.LogThresholdReached(cpuUsage, _baseServerEventLogger);
            }
        }
    }
}