using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServerSightAPI.Controllers;
using ServerSightAPI.Models;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Middleware
{
    // usage [ApiKeyAuthorization] on any given route
    public class ApiKeyAuthorization :  AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly ILogger<ApiKeyAuthorization> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public ApiKeyAuthorization(
            ILogger<ApiKeyAuthorization> logger, 
            IUnitOfWork unitOfWork,
            UserManager<User> userManager
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            
            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var apiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (apiKey.Count == 0 || string.IsNullOrWhiteSpace(apiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var apiKeyDb = await _unitOfWork.ApiKeys.Get(q => q.Key == apiKey, includes: new List<string>
            {
                "User"
            });

            var user = await _userManager.FindByEmailAsync(apiKeyDb.OwnedBy.Email);
            var roles = await _userManager.GetRolesAsync(user);
            context.HttpContext.User = new GenericPrincipal(new ClaimsIdentity(user.Email), roles.ToArray());
        }
    }
}