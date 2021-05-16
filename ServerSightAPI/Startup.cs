using System;
using System.Net;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ServerSightAPI.Background;
using ServerSightAPI.Configurations;
using ServerSightAPI.Configurations.Services;
using ServerSightAPI.EventLoggers;
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

            services.ConfigureCorsHeaders();
            services.AddHttpContextAccessor();

            services.ConfigureJwt(Configuration);

            // provides an instances when the application ask one to inject
            services.AddTransient<IBaseServerEventLogger, BaseServerEventLogger>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<IServerPowerStatusSetter, ServerPowerStatusSetter>();
            
            services.AddAutoMapper(typeof(MapperInitializer));

            services.ConfigureSwagger();

            services.AddMemoryCache();
            services.ConfigureModelStateHandler();
            services.ConfigureFirebase(Configuration);
            
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("192.168.2.198"));
                options.KnownProxies.Add(IPAddress.Parse("192.168.2.86"));
                options.KnownProxies.Add(IPAddress.Parse("0.0.0.0"));
            });
            // hangfire is used for background tasks.
            services.ConfigureHangFire(Configuration);
            
            services.AddControllers().AddNewtonsoftJson(op =>
                op.SerializerSettings.ReferenceLoopHandling =
                    ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // in production you should something like nginx or caddy
                app.UseConfiguredStaticFiles(env);
            }

            app.UseCors("CorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUi();

            app.UseHttpsRedirection();
            app.ConfigureExceptionHandler();

            app.UseRouting();

            app.UseResponseCaching();

            // for nginx reverse proxy header passing.
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            app.UseAuthorization();


            try
            {
                app.UseHangfireServer();
                app.UseHangfireDashboard("/hangfire-jobs", new DashboardOptions
                {
                    IsReadOnlyFunc = (DashboardContext context) => true
                });
            }
            catch (Exception exception)
            {
                Console.WriteLine("Failed to start hangfire service");
            }

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}