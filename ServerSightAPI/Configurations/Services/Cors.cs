using Microsoft.Extensions.DependencyInjection;

namespace ServerSightAPI.Configurations.Services
{
    public static class Cors
    {
        public static void ConfigureCORSHeaders(this IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }
    }
}