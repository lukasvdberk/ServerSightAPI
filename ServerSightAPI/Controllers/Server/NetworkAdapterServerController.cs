using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerSightAPI.DTO.Server;
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
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public NetworkAdapterServerController(
            ILogger<NetworkAdapterServerController> logger, 
            UserManager<User> userManager,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IList<NetworkAdapterServerDto>> GetNetworkServerAdapters(Guid serverId)
        {
            var server = await GetUserHisServer(serverId);

            var networkAdapterServers = await _unitOfWork.NetworkAdaptersServer.GetAll(
                q => q.ServerId == server.Id
            );
            var networkAdaptersServersMapped = _mapper.Map<IList<NetworkAdapterServerDto>>(networkAdapterServers);

            return networkAdaptersServersMapped;
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
            var server = await GetUserHisServer(serverId);
            
            if (server == null)
            {
                return BadRequest("Sever id is either null or you are not the owner of the server.");
            }
            
            var networkAdaptersOfServer = _mapper.Map<IList<NetworkAdapterServer>>(networkAdapterServerDto);
            // set the right server id
            networkAdaptersOfServer.ForAll(s => s.ServerId = serverId.ToString());

            // so we overwrite the existing.
            await RemoveExistingNetworkAdaptersOfServer(serverId.ToString());
            await _unitOfWork.NetworkAdaptersServer.InsertRange(networkAdaptersOfServer);
            return NoContent();
        }
        
        // TODO refactor to generic utility function
        private async Task<Server> GetUserHisServer(Guid serverId)
        {
            if (HttpContext.Request.Headers.TryGetValue("X-Api-Key", out var apiKey))
            {
                string apiKeyStr = apiKey.ToString();
                var apiKeyDb = await _unitOfWork.ApiKeys.Get(q => q.Key == apiKeyStr);
            
                // only fetch servers from the user who requested it
                return await _unitOfWork.Servers.Get(expression: q =>
                    q.OwnedById == apiKeyDb.OwnedById && q.Id == serverId.ToString()
                );
            }

            return null;
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