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
using ServerSightAPI.Middleware;
using ServerSightAPI.Models;
using ServerSightAPI.Models.Server;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Controllers
{
    [ApiController]
    [Route("api/servers/{serverId:Guid}/ports")]
    public class PortServerController : ControllerBase
    {
        private readonly ILogger<PortServerController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PortServerController(
            ILogger<PortServerController> logger,
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IList<PortServer>> GetServerPorts(Guid serverId)
        {
            var server = await ServerUtilController.GetUserHisServerFromApiKey(serverId, HttpContext);

            var portsOfServer = await _unitOfWork.PortsServer.GetAll(
                q => q.ServerId == server.Id
            );

            return portsOfServer;
        }

        [HttpPut]
        [ApiKeyAuthorization]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetOpenPortsForServer(
            Guid serverId,
            [FromBody] IList<PortServer> portServersDto
        )
        {
            var server = await ServerUtilController.GetUserHisServerFromApiKey(serverId, HttpContext);

            if (server == null) return BadRequest("Sever id is either null or you are not the owner of the server.");

            var portServers = _mapper.Map<IList<PortServer>>(portServersDto);
            // set the right server id
            portServers.ForAll(s => s.ServerId = serverId.ToString());

            // so we overwrite the existing.
            await RemoveExistingPortsOfServer(serverId.ToString());
            await _unitOfWork.PortsServer.InsertRange(portServers);
            return NoContent();
        }


        private async Task RemoveExistingPortsOfServer(string serverId)
        {
            var existingPorts = await _unitOfWork.PortsServer.GetAll(
                q => q.ServerId == serverId
            );

            _unitOfWork.PortsServer.DeleteRange(existingPorts);
        }
    }
}