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
                var postgreSqlConnection = new NpgsqlConnection(configuration.GetConnectionString("sqlConnection"));
                options.UseNpgsql(postgreSqlConnection);
            });

        }
    }
}