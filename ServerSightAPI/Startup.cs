using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ServerSightAPI.Configurations;
using ServerSightAPI.Configurations.Services;
using ServerSightAPI.Repository;
using ServerSightAPI.Services;

namespace ServerSightAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigurePostgreDatabase(Configuration);

            services.AddAuthentication();
            services.AddAuthorization();
            services.ConfigureIdentity();
            
            services.AddControllers();
            
            services.ConfigureCORSHeaders();
            services.AddHttpContextAccessor();
            
            services.ConfigureJwt(Configuration);
            
            // provides an instances when the application aks one to inject
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthManager, AuthManager>();
            
            services.AddAutoMapper(typeof(MapperInitializer));
            
            services.ConfigureSwagger();
            
            services.AddMemoryCache();
            
            services.AddControllers().AddNewtonsoftJson(op => 
                op.SerializerSettings.ReferenceLoopHandling = 
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUi();
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseResponseCaching();
            
            // for nginx reverse proxy header passing.
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}