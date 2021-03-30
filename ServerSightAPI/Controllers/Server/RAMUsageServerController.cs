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

        public RamUsageController(
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
        public async Task<IList<RamUsageDto>> GetRamUsageOfServer(Guid serverId, [FromQuery] CreatedBetween createdBetween)
        {
            var server = await ServerUtilController.GetServerFromJwt(serverId, HttpContext);

            var ramUsages = await _unitOfWork.RAMUsages.GetAll(
                q => q.ServerId == server.Id 
                     && createdBetween.From >= q.CreatedAt && createdBetween.To <= q.CreatedAt
            );

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

            return Ok();
        }
    }
}