using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace ServerSightAPI.Configurations.Services
{
    public static class Database
    {
        public static void ConfigurePostgreDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                try
                {
                    var postgreSqlConnection = configuration.GetConnectionString("sqlConnection");
                    if (!string.IsNullOrEmpty(postgreSqlConnection))
                    {
                        options.UseNpgsql(new NpgsqlConnection(postgreSqlConnection));
                    }
                    else
                    {
                        Console.WriteLine("Could not connect to postgres database! Please check your configuration");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Could not connect to postgres database! Please check your configuration");
                }
            });
        }
    }
}