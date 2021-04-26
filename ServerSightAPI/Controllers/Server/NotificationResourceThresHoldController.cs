using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerSightAPI.DTO.Server;
using ServerSightAPI.DTO.Server.NetworkAdapterServer;
using ServerSightAPI.Models.Server;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Controllers
{
    [ApiController]
    [Route("api/servers/{serverId:Guid}/notifications/threshold")]
    public class NotificationResourceThresHoldController : ControllerBase
    {
        private readonly ILogger<NotificationResourceThresHoldController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationResourceThresHoldController(
            ILogger<NotificationResourceThresHoldController> logger,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNotificationTresholdSettings(Guid serverId)
        {
            var server = await ServerUtilController.GetServerFromJwt(serverId, HttpContext);

            if (server == null) return BadRequest("Server id is either null or you are not the owner");
            
            var notificationResourceThreshold = await _unitOfWork.NotificationThresholds.Get(
                q => q.ServerId == server.Id
            );
            
            return Ok(notificationResourceThreshold);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveAndRemoveExistingNotificationTresholds(Guid serverId, 
            [FromBody] NotificationResourceThresholdDTO notificationResourceThresholdDto)
        {
            var server = await ServerUtilController.GetServerFromJwt(serverId, HttpContext);
            
            if (server == null) return BadRequest("Server id is either null or you are not the owner");

            var notificationResourceThresholdToDelete =
                await _unitOfWork.NotificationThresholds.GetAll(q => q.ServerId == server.Id
            );

            _unitOfWork.NotificationThresholds.DeleteRange(notificationResourceThresholdToDelete);
            
            var notificationResourceThresholdToSave =
                _mapper.Map<NotificationResourceThreshold>(notificationResourceThresholdDto);
            notificationResourceThresholdToSave.ServerId = server.Id;

            await _unitOfWork.NotificationThresholds.Insert(notificationResourceThresholdToSave);
            
            return NoContent();
        }
    }
}