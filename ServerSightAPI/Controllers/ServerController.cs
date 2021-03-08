using System;
using System.Collections;
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
using ServerSightAPI.DTO.User;
using ServerSightAPI.Models;
using ServerSightAPI.Models.Server;
using ServerSightAPI.Repository;
using ServerSightAPI.Services;

namespace ServerSightAPI.Controllers
{
    [ApiController]
    [Route("api/servers")]
    public class ServerController : ControllerBase
    {
        private readonly ILogger<ServerController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        
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
            // TODO make separate function maybe?
            var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);

            // (q.PowerStatus == searchServerDto.PowerStatus || q.Title.Contains(searchServerDto.Title)

            // only fetch servers from the user who requested it
            var servers = await _unitOfWork.Servers.GetAll(expression: q => q.OwnedById == user.Id);

            if (!string.IsNullOrWhiteSpace(searchServerDto.Title))
            {
                servers = servers.Where(s => s.Title.Contains(searchServerDto.Title)).ToList();
            }

            servers = servers.Where(s => s.PowerStatus == searchServerDto.PowerStatus).ToList();

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
            // TODO make separate function maybe?
            var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            
            // only fetch servers from the user who requested it
            var server = await _unitOfWork.Servers.Get(expression: q => 
                q.OwnedById == user.Id && q.Id == id.ToString()
            );
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
            // TODO make separate function maybe?
            var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            var server = _mapper.Map<Server>(serverDto);
            
            server.CreatedAt = DateTime.Now;

            server.OwnedById = user.Id;
            await _unitOfWork.Servers.Insert(server);
            
            return _mapper.Map<ServerDto>(server);
        }

        [HttpPut("image/{id:Guid}"), DisableRequestSizeLimit]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddImageToServer([FromForm] IFormFile file, Guid id)
        {
            // TODO make separate function maybe?
            var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            
            // only fetch servers from the user who requested it
            var server = await _unitOfWork.Servers.Get(expression: q => 
                q.OwnedById == user.Id && q.Id == id.ToString()
            );

            if (server != null)
            {
                string fName = file.FileName;
                string path = Path.Combine("Images/" + file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return Ok();    
            }

            return BadRequest();
        }
    }
}