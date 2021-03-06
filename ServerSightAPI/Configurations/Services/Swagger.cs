using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ServerSightAPI.Configurations.Services
{
    public static class Swagger
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Server sight REST API",
                    Version = "v1"
                });
            });
        }

        public static void UseSwaggerUi(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(c =>
            {
                // will fail to load the right json file in production if not set this way.
                var swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "server-sight-v1");
            });
        }
    }
}