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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServerSightAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServerSightAPI.Models;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Middleware
{
    // usage [ApiKeyAuthorization] on any given route
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthorization :  Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userManager = context.HttpContext.RequestServices.GetService(typeof(UserManager<User>)) as UserManager<User>;
            var unitOfWork = context.HttpContext.RequestServices.GetService(typeof(IUnitOfWork)) as IUnitOfWork;

            // TODO configure via appsettings or something
            if (context.HttpContext.Request.Headers.TryGetValue("X-Api-Key", out var apiKey))
            {
                if (apiKey.Count == 0 || string.IsNullOrWhiteSpace(apiKey))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
                string apiKeyStr = apiKey.ToString();

                var apiKeyDb = await unitOfWork.ApiKeys.Get(q => q.Key == apiKeyStr);

                if (apiKeyDb == null)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
                var user = await userManager.FindByIdAsync(apiKeyDb.OwnedById);
                var roles = await userManager.GetRolesAsync(user);
                context.HttpContext.User = new GenericPrincipal(new ClaimsIdentity(user.Email), roles.ToArray());   
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}