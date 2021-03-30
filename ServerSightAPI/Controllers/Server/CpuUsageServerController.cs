using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerSightAPI.DTO;
using ServerSightAPI.DTO.Server;
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

        public CpuUsageServerController(
            ILogger<CpuUsageServerController> logger,
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
        public async Task<IList<CpuUsageServer>> GetCpuUsageOfServer(Guid serverId, [FromQuery] CreatedBetween createdBetween)
        {
            var server = await ServerUtilController.GetServerFromJwt(serverId, HttpContext);

            var cpusUsageServers = await _unitOfWork.CpuUsagesServers.GetAll(
                q => q.ServerId == server.Id 
                     && createdBetween.From >= server.CreatedAt && createdBetween.To <= server.CreatedAt
            );
            // todo setup mapping.
            return cpusUsageServers;
        }
        
        [HttpPost]
        [ApiKeyAuthorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveCpuUsageMinuteOfServer(Guid serverId, [FromBody] CpuUsageDto cpuUsageDto)
        {
            var server = await ServerUtilController.GetUserHisServerFromApiKey(serverId, HttpContext);

            var cpuUsage = _mapper.Map<CpuUsageServer>(cpuUsageDto);
            cpuUsage.ServerId = server.Id;
            cpuUsage.CreatedAt = DateTime.Now;
            
            await _unitOfWork.CpuUsagesServers.Insert(cpuUsage);

            return Ok();
        }
    }
}