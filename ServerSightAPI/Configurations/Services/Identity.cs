using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ServerSightAPI.Models;

namespace ServerSightAPI.Configurations.Services
{
    public static class Identity
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>(
                q => q.User.RequireUniqueEmail = true);
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
        }
    }
}