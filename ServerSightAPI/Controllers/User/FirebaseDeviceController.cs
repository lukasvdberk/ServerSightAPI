using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerSightAPI.DTO.ApiKey;
using ServerSightAPI.DTO.User;
using ServerSightAPI.Models;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Controllers
{
    /**
     * For registering firebase devices.
     * This is currently used to for push notifications.
     */
    public class FirebaseDeviceController : ControllerBase
    {
        private readonly ILogger<UserApiController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public FirebaseDeviceController(
            ILogger<UserApiController> logger,
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
        
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApiKey([FromBody] CreateFirebaseDTO createFirebaseDto)
        {
            var user = await GetUser();

            var firebaseDevice = _mapper.Map<FirebaseDevice>(createFirebaseDto);
            firebaseDevice.OwnedById = user.Id;

            await _unitOfWork.FirebaseDevices.Insert(firebaseDevice);
            return NoContent();
        }

        private async Task<User> GetUser()
        {
            return await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }
    }
}