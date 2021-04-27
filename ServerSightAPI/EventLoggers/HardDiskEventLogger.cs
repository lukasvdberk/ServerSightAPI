using System.Threading.Tasks;
using ServerSightAPI.Controllers;
using ServerSightAPI.Models.Server;

namespace ServerSightAPI.EventLoggers
{
    public class HardDiskEventLogger
    {
        public static async Task LogThresholdReached(Server server, HardDiskServer hardDiskServer, IBaseServerEventLogger serverEventLogger)
        {
            var usageInPercentage = HardDiskServerController.HardDiskUsageInPercentage(hardDiskServer).ToString ("#.#");

            await serverEventLogger.LogEvent(
                $@"Hard disk threshold reached for {server.Name} server",
                $@"{server.Name} server reached hard disk threshold. Current disk usage is {usageInPercentage}%", 
                EventType.HardDiskThresholdReached, 
                server
            );
        }
    }
}