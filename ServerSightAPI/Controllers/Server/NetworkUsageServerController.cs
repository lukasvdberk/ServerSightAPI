using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerSightAPI.DTO;
using ServerSightAPI.DTO.Server;
using ServerSightAPI.DTO.Server.NetworkUsage;
using ServerSightAPI.Middleware;
using ServerSightAPI.Models.Server;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Controllers
{
    [ApiController]
    [Route("api/servers/{serverId:Guid}/network-usages")]
    public class NetworkUsageServerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public NetworkUsageServerController(
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IList<NetworkUsageDTO>> GetRamUsageOfServer(Guid serverId, [FromQuery] CreatedBetween createdBetween)
        {
            var server = await ServerUtilController.GetServerFromJwt(serverId, HttpContext);

            var networkUsages = await _unitOfWork.NetworkUsages.GetAll(
                q => q.ServerId == server.Id 
                     && createdBetween.From >= q.CreatedAt && createdBetween.To <= q.CreatedAt
            );
            
            // TODO refactor (or on unit of work layer)
            // year 1 is the default which means the user dit not provide a year.
            if (createdBetween.From.Year != 1 && createdBetween.From.Year != 1)
            {
                networkUsages = await _unitOfWork.NetworkUsages.GetAll(
                    q => 
                        server.Id == q.ServerId &&
                        q.CreatedAt >= createdBetween.From && q.CreatedAt <= createdBetween.To
                );
            }
            return _mapper.Map<IList<NetworkUsageDTO>>(networkUsages);
        }
        
        [HttpPost]
        [ApiKeyAuthorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveNetworkUSage(Guid serverId, [FromBody] CreateNetworkUsageDto networkUsageDto)
        {
            // TODO check if not something was already posted in the past minute
            var server = await ServerUtilController.GetUserHisServerFromApiKey(serverId, HttpContext);

            var networkUsage = _mapper.Map<NetworkUsage>(networkUsageDto);
            networkUsage.ServerId = server.Id;
            networkUsage.CreatedAt = DateTime.Now;
            
            await _unitOfWork.NetworkUsages.Insert(networkUsage);

            return Ok();
        }
    }
}