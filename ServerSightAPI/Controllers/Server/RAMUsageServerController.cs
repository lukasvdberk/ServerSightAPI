using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerSightAPI.DTO;
using ServerSightAPI.DTO.Server;
using ServerSightAPI.EventLoggers;
using ServerSightAPI.Middleware;
using ServerSightAPI.Models.Server;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Controllers
{
    [ApiController]
    [Route("api/servers/{serverId:Guid}/ram")]
    public class RamUsageController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseServerEventLogger _baseServerEventLogger;

        public RamUsageController(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IBaseServerEventLogger baseServerEventLogger
        )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _baseServerEventLogger = baseServerEventLogger;
        }
        
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IList<RamUsageDto>> GetRamUsageOfServer(Guid serverId, [FromQuery] CreatedBetween createdBetween)
        {
            var server = await ServerUtilController.GetServerFromJwt(serverId, HttpContext);

            var ramUsages = await _unitOfWork.RAMUsages.GetAll(
                q => q.ServerId == server.Id 
                     && createdBetween.From >= q.CreatedAt && createdBetween.To <= q.CreatedAt
            );
            
            // TODO refactor (or on unit of work layer)
            // year 1 is the default which means the user dit not provide a year.
            if (createdBetween.From.Year != 1 && createdBetween.From.Year != 1)
            {
                ramUsages = await _unitOfWork.RAMUsages.GetAll(
                    q => 
                        server.Id == q.ServerId &&
                        q.CreatedAt >= createdBetween.From && q.CreatedAt <= createdBetween.To
                );
            }
            return _mapper.Map<IList<RamUsageDto>>(ramUsages);
        }
        
        [HttpPost]
        [ApiKeyAuthorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveRamUsageMinuteOfServer(Guid serverId, [FromBody] CreateRamUsageDto ramUsageDto)
        {
            // TODO check if not something was already posted in the past minute
            var server = await ServerUtilController.GetUserHisServerFromApiKey(serverId, HttpContext);

            var ramUsage = _mapper.Map<RamUsage>(ramUsageDto);
            ramUsage.ServerId = server.Id;
            ramUsage.CreatedAt = DateTime.Now;
            
            await _unitOfWork.RAMUsages.Insert(ramUsage);

            ramUsage.Server = server;
            await RAMEventLoggerChecker(ramUsage);
            
            return Ok();
        }
        
        private async Task RAMEventLoggerChecker(RamUsage ramUsage)
        {
            var notificationThreshold = await _unitOfWork.NotificationThresholds.Get(
                q => q.ServerId == ramUsage.ServerId
            );
            
            if(notificationThreshold == null) {return;}
            if (GetRAMUsageInPercent(ramUsage) >= notificationThreshold.RamUsageThresholdInPercentage)
            {
                await RAMServerEventLogger.LogThresholdReached(ramUsage, _baseServerEventLogger);
            }
        }

        public static double GetRAMUsageInPercent(RamUsage ramUsage)
        {
            return ((ramUsage.TotalAvailableInBytes - ramUsage.UsageInBytes) / ramUsage.TotalAvailableInBytes) * 100;
        }
    }
}