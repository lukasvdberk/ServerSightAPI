using System.Threading.Tasks;
using ServerSightAPI.Controllers;
using ServerSightAPI.Models.Server;

namespace ServerSightAPI.EventLoggers
{
    public class PowerStatusServerEventLogger
    {
        public static async Task LogPowerServerStatusChanged(Server server, bool serverOn, IBaseServerEventLogger serverEventLogger)
        {
            var onOffText = serverOn ? "is back online" : "went offline";

            var text = $@"Server {server.Name} {onOffText}";
            await serverEventLogger.LogEvent(
                text,
                text,
                EventType.PowerStatus, 
                server
            );
        }
    }
}