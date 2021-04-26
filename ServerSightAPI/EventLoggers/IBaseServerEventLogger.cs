using System.Threading.Tasks;
using ServerSightAPI.Models.Server;

namespace ServerSightAPI.EventLoggers
{
    public interface IBaseServerEventLogger
    {
        public Task LogEvent(string description, EventType eventType, Server server);
    }
}