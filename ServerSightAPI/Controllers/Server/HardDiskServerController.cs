using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerSightAPI.DTO.Server.HardDiskServer;
using ServerSightAPI.EventLoggers;
using ServerSightAPI.Middleware;
using ServerSightAPI.Models;
using ServerSightAPI.Models.Server;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Controllers
{
    [ApiController]
    [Route("api/servers/{serverId:Guid}/hard-disks")]
    public class HardDiskServerController : ControllerBase
    {
        private readonly ILogger<HardDiskServerController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IBaseServerEventLogger _baseServerEventLogger;

        public HardDiskServerController(
            ILogger<HardDiskServerController> logger,
            UserManager<User> userManager,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IBackgroundJobClient backgroundJobClient,
            IBaseServerEventLogger baseServerEventLogger
        )
        {
            _logger = logger;
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
        public async Task<IList<HardDiskServer>> GetHardDisksOfServer(Guid serverId)
        {
            var server = await ServerUtilController.GetServerFromJwt(serverId, HttpContext);

            var hardDiskServers = await _unitOfWork.HardDisksServers.GetAll(
                q => q.ServerId == server.Id
            );

            return hardDiskServers;
        }

        [HttpPut]
        [ApiKeyAuthorization]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetHardDisksForServer(
            Guid serverId,
            [FromBody] IList<CreateHardDiskServer> hardDiskServersDto
        )
        {
            var server = await ServerUtilController.GetUserHisServerFromApiKey(serverId, HttpContext);

            if (server == null) return BadRequest("Sever id is either null or you are not the owner of the server.");

            var hardDiskServers = _mapper.Map<IList<HardDiskServer>>(hardDiskServersDto);
            // set the right server id
            hardDiskServers.ForAll(s => s.ServerId = serverId.ToString());

            // so we overwrite the existing.
            await RemoveExistingHardDisksOfServer(serverId.ToString());
            await _unitOfWork.HardDisksServers.InsertRange(hardDiskServers);
            
            await HardDiskEventChecker(server, hardDiskServers);
            return NoContent();
        }


        private async Task RemoveExistingHardDisksOfServer(string serverId)
        {
            var existingHardDisks = await _unitOfWork.HardDisksServers.GetAll(
                q => q.ServerId == serverId
            );

            _unitOfWork.HardDisksServers.DeleteRange(existingHardDisks);
        }
        
        /**
         * Check whether a event for the hard disk resource should be fired.
         * If a hard disk meets the criteria for a event it will throw it in this method.
         */
        private async Task HardDiskEventChecker(Server server, IList<HardDiskServer> hardDisksServer)
        {
            var notificationThreshold = await _unitOfWork.NotificationThresholds.Get(
                q => q.ServerId == server.Id);

            foreach (var hardDiskServer in hardDisksServer)
            {
                var usageInPercentage = (hardDiskServer.SpaceAvailable / hardDiskServer.SpaceTotal) * 100;

                if (usageInPercentage >= notificationThreshold.HardDiskUsageThresholdInPercentage)
                {
                    await HardDiskEventLogger.LogThresholdReached(server, hardDiskServer, _baseServerEventLogger);
                }
            }

        }

        public static double HardDiskUsageInPercentage(HardDiskServer hardDiskServer)
        {
            return ((hardDiskServer.SpaceTotal - hardDiskServer.SpaceAvailable) / hardDiskServer.SpaceTotal) * 100;
        }
    }
}