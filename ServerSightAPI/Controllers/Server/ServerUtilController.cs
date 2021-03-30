using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ServerSightAPI.Models;
using ServerSightAPI.Models.Server;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Controllers
{
    public class ServerUtilController
    {
        /*
         * Extracts user from X-Api-Key (needs to be valid) and get the server.
         */
        public static async Task<Server> GetUserHisServerFromApiKey(Guid serverId, HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("X-Api-Key", out var apiKey))
            {
                var apiKeyStr = apiKey.ToString();
                var unitOfWork = httpContext.RequestServices.GetService(typeof(IUnitOfWork)) as IUnitOfWork;

                var apiKeyDb = await unitOfWork.ApiKeys.Get(q => q.Key == apiKeyStr);

                // only fetch servers from the user who requested it
                return await unitOfWork.Servers.Get(q =>
                    q.OwnedById == apiKeyDb.OwnedById && q.Id == serverId.ToString()
                );
            }

            return null;
        }
        /*
         * Extracts user and retrieves the server from the user (if they are the owner)
         */
        public static async Task<Server> GetServerFromJwt(Guid serverId, HttpContext httpContext)
        {
            var userManager =
                httpContext.RequestServices.GetService(typeof(UserManager<User>)) as UserManager<User>;
            var unitOfWork = httpContext.RequestServices.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            var user = await userManager.FindByEmailAsync(httpContext.User.Identity.Name);

            // only fetch servers from the user who requested it
            return await unitOfWork.Servers.Get(q =>
                q.OwnedById == user.Id && q.Id == serverId.ToString()
            );
        }
    }
}