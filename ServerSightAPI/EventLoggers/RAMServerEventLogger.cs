using System.Threading.Tasks;
using ServerSightAPI.Models.Server;

namespace ServerSightAPI.EventLoggers
{
    public class RAMServerEventLogger
    {
        public static async Task LogThresholdReached(RamUsage ramUsage, IBaseServerEventLogger serverEventLogger)
        {
            var server = ramUsage.Server;
            var usageInPercentage = ramUsage.UsageInBytes.ToString ("#.#");

            await serverEventLogger.LogEvent(
                $@"CPU threshold reached for {server.Name} server",
                $@"{server.Name} server reached CPU usage threshold. Current CPU usage is {usageInPercentage}%", 
                EventType.CPUUsageThresholdReached, 
                server
            );
        }
    }
}