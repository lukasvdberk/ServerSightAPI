using ServerSightAPI.Models.Server;

namespace ServerSightAPI.EventLoggers
{
    public class HardDiskEventLogger
    {
        public static async void LogThresholdReached(Server server, HardDiskServer hardDiskServer, IBaseServerEventLogger serverEventLogger)
        {
            var usageInPercentage = (hardDiskServer.SpaceAvailable / hardDiskServer.SpaceTotal) * 100;

            await serverEventLogger.LogEvent(
                $@"Server {server.Name} reached hard disk threshold. Current disk usage is {usageInPercentage}", 
                EventType.HardDiskThresholdReached, 
                server
            );
        }
    }
}