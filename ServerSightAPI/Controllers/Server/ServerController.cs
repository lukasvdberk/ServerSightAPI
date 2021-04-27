using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerSightAPI.DTO.Server;
using ServerSightAPI.Models;
using ServerSightAPI.Models.Server;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Controllers
{
    [ApiController]
    [Route("api/servers")]
    public class ServerController : ControllerBase
    {
        private readonly ILogger<ServerController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public ServerController(
            ILogger<ServerController> logger,
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
        public async Task<IList<ServerDto>> GetServersOfUser([FromQuery] SearchServerDto searchServerDto)
        {
            var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);

            // only fetch servers from the user who requested it
            var servers = await _unitOfWork.Servers.GetAll(q => q.OwnedById == user.Id,
                orderBy => orderBy.OrderByDescending(s => s.CreatedAt)
            );

            if (!string.IsNullOrWhiteSpace(searchServerDto.Name))
                servers = servers.Where(s => s.Name.Contains(searchServerDto.Name)).ToList();

            if (searchServerDto.PowerStatus != null)
                servers = servers.Where(s => s.PowerStatus == searchServerDto.PowerStatus).ToList();
            if (!string.IsNullOrWhiteSpace(searchServerDto.Ip))
            {
                var networkAdapterServers = await _unitOfWork.NetworkAdaptersServer.GetAll(q =>
                    q.Ip == searchServerDto.Ip
                );
                servers = servers.Where(
                    s => 
                        networkAdapterServers.Any(n => n.ServerId == s.Id)
                ).ToList();
            }

            var serversMapped = _mapper.Map<IList<ServerDto>>(servers);

            return serversMapped;
        }

        [HttpGet("{id:Guid}", Name = "GetServer")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ServerDto> GetServer(Guid id)
        {
            var server = await GetUserHisServer(id);

            var serverMapped = _mapper.Map<ServerDto>(server);

            return serverMapped;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ServerDto> SaveServer([FromBody] CreateServerDto serverDto)
        {
            var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            var server = _mapper.Map<Server>(serverDto);

            server.CreatedAt = DateTime.Now;

            server.OwnedById = user.Id;
            await _unitOfWork.Servers.Insert(server);
            // so that there is always a default configuration for notification thresholds
            await SetDefaultServerNotifications(server);
            
            return _mapper.Map<ServerDto>(server);
        }

        [HttpPut("{id:Guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveServer(Guid id, [FromBody] UpdateServerDto serverDto)
        {
            var server = await GetUserHisServer(id);

            if (server == null) return Unauthorized();

            var updatedServer = _mapper.Map<Server>(serverDto);
            updatedServer.Id = id.ToString();
            updatedServer.OwnedById = server.OwnedById;
            updatedServer.ImagePath = server.ImagePath;

            _unitOfWork.Servers.Update(updatedServer);

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveServer(Guid id)
        {
            var server = await GetUserHisServer(id);

            if (server == null) return Unauthorized();

            await _unitOfWork.Servers.Delete(id.ToString());

            return NoContent();
        }

        [HttpPut("image/{id:Guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddImageToServer(Guid id, [FromForm] IFormFile file)
        {
            var server = await GetUserHisServer(id);

            var formCollection = await Request.ReadFormAsync();
            var serverImage = formCollection.Files.First();

            if (server == null) return BadRequest("Server id is either null or you are not the owner of the server.");

            // TODO set folder from configuration
            var relativePath = "Resources/Images/" + serverImage.FileName;
            var path = Path.Combine(relativePath);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await serverImage.CopyToAsync(stream);
            }

            server.ImagePath = "Images/" + serverImage.FileName; ;
            _unitOfWork.Servers.Update(server);
            return Ok();
        }

        private async Task SetDefaultServerNotifications(Server server)
        {
            await _unitOfWork.NotificationThresholds.Insert(new NotificationResourceThreshold()
            {
                CpuUsageThresholdInPercentage = 80,
                HardDiskUsageThresholdInPercentage = 80,
                RamUsageThresholdInPercentage = 80,
                ServerId = server.Id
            });
        }

        private async Task<Server> GetUserHisServer(Guid serverId)
        {
            var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);

            // only fetch servers from the user who requested it
            return await _unitOfWork.Servers.Get(q =>
                q.OwnedById == user.Id && q.Id == serverId.ToString()
            );
        }
    }
}