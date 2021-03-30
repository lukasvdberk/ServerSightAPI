using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


namespace ServerSightAPI.Configurations.Services
{
    public static class HangFireScheduler
    {
        public static void ConfigureHangFire(this IServiceCollection services, IConfiguration configuration)
        {
            // hangfire is for background tasks
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(configuration.GetConnectionString("hangFirePostgreConnection")));
        }
    }
}