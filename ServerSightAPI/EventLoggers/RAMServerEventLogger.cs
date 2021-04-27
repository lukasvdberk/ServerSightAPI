using System.Threading.Tasks;
using ServerSightAPI.Controllers;
using ServerSightAPI.Models.Server;

namespace ServerSightAPI.EventLoggers
{
    public class RAMServerEventLogger
    {
        public static async Task LogThresholdReached(RamUsage ramUsage, IBaseServerEventLogger serverEventLogger)
        {
            var server = ramUsage.Server;
            var usageInPercentage = RamUsageController.GetRAMUsageInPercent(ramUsage).ToString ("#.#");

            await serverEventLogger.LogEvent(
                $@"RAM threshold reached for {server.Name} server",
                $@"{server.Name} server reached RAM usage threshold. Current RAM usage is {usageInPercentage}%", 
                EventType.RAMUsageThresholdReached, 
                server
            );
        }
    }
}