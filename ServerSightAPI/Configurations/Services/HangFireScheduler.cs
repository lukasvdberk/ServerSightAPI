using System;
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
            try
            {
                // hangfire is for background tasks
                services.AddHangfire(config =>
                    config.UsePostgreSqlStorage(configuration.GetConnectionString("hangFirePostgreConnection")));
            }
            catch (Exception exception)
            {
                Console.WriteLine("Failed to setup database for hangfire. Check your configuration!");
            }
        }
    }
}