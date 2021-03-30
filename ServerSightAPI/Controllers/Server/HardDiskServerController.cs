using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerSightAPI.DTO.Server.HardDiskServer;
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

        public HardDiskServerController(
            ILogger<HardDiskServerController> logger,
            UserManager<User> userManager,
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
            return NoContent();
        }


        private async Task RemoveExistingHardDisksOfServer(string serverId)
        {
            var existingHardDisks = await _unitOfWork.HardDisksServers.GetAll(
                q => q.ServerId == serverId
            );

            _unitOfWork.HardDisksServers.DeleteRange(existingHardDisks);
        }
    }
}