using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace ServerSightAPI.Configurations.Services
{
    public static class StaticFiles
    {
        public static void UseConfiguredStaticFiles(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Resources/")),
                RequestPath = "/assets"
            });
        }
    }
}