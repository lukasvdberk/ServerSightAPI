using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerSightAPI.DTO.Server;
using ServerSightAPI.DTO.Server.ServerEvents;
using ServerSightAPI.Models.Server;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Controllers
{
    
    [ApiController]
    [Route("api/servers/{serverId:Guid}/events")]
    public class ServerEventsController : ControllerBase 
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ServerEventsController(
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
        public async Task<IList<ServerEventDto>> GetEventsOfServer(Guid serverId)
        {
            var server = await ServerUtilController.GetServerFromJwt(serverId, HttpContext);
            var serverEvents = await _unitOfWork.ServerEvents.GetAll(
                q => q.ServerId == server.Id,
                orderBy => orderBy.OrderByDescending(s => s.CreatedAt) 
                
            );

            return _mapper.Map<IList<ServerEventDto>>(serverEvents);
        }
    }
}