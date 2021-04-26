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
using ServerSightAPI.DTO.Server.NetworkAdapterServer;
using ServerSightAPI.Middleware;
using ServerSightAPI.Models;
using ServerSightAPI.Models.Server;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Controllers
{
    [ApiController]
    [Route("api/servers/{serverId:Guid}/ips")]
    public class NetworkAdapterServerController : ControllerBase
    {
        private readonly ILogger<NetworkAdapterServerController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public NetworkAdapterServerController(
            ILogger<NetworkAdapterServerController> logger,
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
        public async Task<IActionResult> GetNetworkServerAdapters(Guid serverId)
        {
            var server = await ServerUtilController.GetServerFromJwt(serverId, HttpContext);

            if (server == null) return BadRequest("Server id is either null or you are not the owner");
            var networkAdapterServers = await _unitOfWork.NetworkAdaptersServer.GetAll(
                q => q.ServerId == server.Id
            );
            var networkAdaptersServersMapped = _mapper.Map<IList<NetworkAdapterServerDto>>(networkAdapterServers);

            return Ok(networkAdaptersServersMapped);
        }

        [HttpPut]
        [ApiKeyAuthorization]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetNetworkAdapterServer(
            Guid serverId,
            [FromBody] IList<CreateNetworkAdapterServerDto> networkAdapterServerDto
        )
        {
            var server = await ServerUtilController.GetUserHisServerFromApiKey(serverId, HttpContext);

            if (server == null) return BadRequest("Server id is either null or you are not the owner of the server.");

            var networkAdaptersOfServer = _mapper.Map<IList<NetworkAdapterServer>>(networkAdapterServerDto);
            // set the right server id
            networkAdaptersOfServer.ForAll(s => s.ServerId = serverId.ToString());

            // so we overwrite the existing.
            await RemoveExistingNetworkAdaptersOfServer(serverId.ToString());
            await _unitOfWork.NetworkAdaptersServer.InsertRange(networkAdaptersOfServer);
            return NoContent();
        }

        private async Task RemoveExistingNetworkAdaptersOfServer(string serverId)
        {
            var existingNetworkAdapters = await _unitOfWork.NetworkAdaptersServer.GetAll(
                q => q.ServerId == serverId
            );

            _unitOfWork.NetworkAdaptersServer.DeleteRange(existingNetworkAdapters);
        }
    }
}