using System.Threading.Tasks;
using ServerSightAPI.Controllers;
using ServerSightAPI.Models.Server;

namespace ServerSightAPI.EventLoggers
{
    public class CPUServerEventLogger
    {
        public static async Task LogThresholdReached(CpuUsageServer cpuUsageServer, IBaseServerEventLogger serverEventLogger)
        {
            var server = cpuUsageServer.Server;
            var usageInPercentage = cpuUsageServer.AverageCpuUsagePastMinute.ToString ("#.#");

            await serverEventLogger.LogEvent(
                $@"CPU threshold reached for {server.Name} server",
                $@"{server.Name} server reached CPU usage threshold. Current CPU usage is {usageInPercentage}%", 
                EventType.CPUUsageThresholdReached, 
                server
            );
        }
    }
}