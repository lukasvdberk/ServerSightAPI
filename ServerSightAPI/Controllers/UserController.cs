using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerSightAPI.DTO.User;
using ServerSightAPI.Models;
using ServerSightAPI.Services;

namespace ServerSightAPI.Controllers
{
    [ApiController]
    [Route("api/user/")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;
        
        public UserController(
            ILogger<UserController> logger, 
            UserManager<User> userManager,
            IMapper mapper,
            IAuthManager authManager
        )
        {
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto)
        {
            _logger.LogInformation($@"Registration attempt for { userDto.Email }");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(userDto);
            
            // email must be unique
            if (await _userManager.FindByEmailAsync(user.Email) != null)
            {
                return BadRequest("User with given email already exists");
            }
            
            // required by entity framework. 
            user.UserName = userDto.Email;
            var result = await _userManager.CreateAsync(user, userDto.Password);
            
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
        
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserDTO userDto)
        {
            _logger.LogInformation($@"Login attempt for {userDto.Email}");
        
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
        
            var user = await _userManager.FindByNameAsync(userDto.Email);
            if (user == null)
            {
                return Unauthorized("Email does not exist");
            }

            if (!await _userManager.CheckPasswordAsync(user, userDto.Password))
            {
                return Unauthorized("Invalid password");
            }

            return Accepted(new
            {
                Token = await _authManager.CreateToken(user)
            });
        }
    }
}