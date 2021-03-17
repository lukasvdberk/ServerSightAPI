using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace ServerSightAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Initialize logger
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                    "logs/log-.txt",
                    outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm.ss.fff zzz} [{Level:u3}] {Message:1j}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information
                ).CreateLogger();
            try
            {
                Log.Information("API is starting!");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "API Failed to start!");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}