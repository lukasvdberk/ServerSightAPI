using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerSightAPI.DTO.ApiKey;
using ServerSightAPI.Models;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Controllers
{
    [ApiController]
    [Route("api/api/keys")]
    public class UserApiController : ControllerBase
    {
        private readonly ILogger<UserApiController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public UserApiController(
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

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IList<ApiKeyDto>> GetApiKey()
        {
            var user = await GetUser();
            var apiKey = await _unitOfWork.ApiKeys.GetAll(q => q.OwnedById == user.Id);
            return _mapper.Map<IList<ApiKeyDto>>(apiKey);
        }


        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteApiKey([FromBody] ApiKeyDto apiKeyDto)
        {
            var user = await GetUser();
            var apiKey = await _unitOfWork.ApiKeys.Get(
                q => q.OwnedById == user.Id && apiKeyDto.Key == q.Key
            );

            await _unitOfWork.ApiKeys.Delete(apiKey.Id);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateApiKey()
        {
            var user = await GetUser();
            var apiKey = new ApiKey();
            var generatedKey = GenerateKey();

            // to ensure the key is not already in use.
            while (!await IsKeyUnique(generatedKey)) generatedKey = GenerateKey();

            apiKey.Key = generatedKey;
            apiKey.CreatedAt = DateTime.Now;
            apiKey.OwnedById = user.Id;

            await _unitOfWork.ApiKeys.Insert(apiKey);
            return Ok(_mapper.Map<ApiKeyDto>(apiKey));
        }

        private string GenerateKey()
        {
            return Guid.NewGuid().ToString("n").Substring(0, 20);
        }

        private async Task<bool> IsKeyUnique(string key)
        {
            return await _unitOfWork.ApiKeys.Get(q => q.Key == key) == null;
        }

        private async Task<User> GetUser()
        {
            return await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }
    }
}