using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ServerSightAPI.Models.Server;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Controllers
{
    public class ServerUtilController
    {
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
    }
}