using System;
using System.Threading.Tasks;
using ServerSightAPI.EventLoggers;
using ServerSightAPI.Models.Server;
using ServerSightAPI.Repository;

namespace ServerSightAPI.Background
{
    public interface IServerPowerStatusSetter { }

    public class ServerPowerStatusSetter : IServerPowerStatusSetter
    {
        private IUnitOfWork _unitOfWork;
        private readonly IBaseServerEventLogger _baseServerEventLogger;

        public ServerPowerStatusSetter(IUnitOfWork unitOfWork, IBaseServerEventLogger baseServerEventLogger)
        {
            _unitOfWork = unitOfWork;
            _baseServerEventLogger = baseServerEventLogger;
        }

        /**
         * Sets the power status to off if no requests from a server was made in the past minute. 
         */
        public async Task SetServerPowerStatus(Server server)
        {
            // update server info from database
            server = await _unitOfWork.Servers.Get(q => server.Id == q.Id);

            if (server != null)
            {
                // checks for past 2 minutes if something was posted.
                var createdBetweenFrom = DateTime.Now - new TimeSpan(0, 1, 0);
                var createdBetweenTo = DateTime.Now;

                var ramUsages = await _unitOfWork.RAMUsages.GetAll(q =>
                    server.Id == q.ServerId &&
                    q.CreatedAt >= createdBetweenFrom && q.CreatedAt <= createdBetweenTo
                );

                var cpuUsages = await _unitOfWork.CpuUsagesServers.GetAll(q =>
                    server.Id == q.ServerId &&
                    q.CreatedAt >= createdBetweenFrom && q.CreatedAt <= createdBetweenTo
                );


                // if no new cpu usage or ram usage was posted in the pas minute that means the server is off.
                if (cpuUsages.Count == 0 || ramUsages.Count == 0)
                {
                    await PowerStatusServerEventLogger.LogPowerServerStatusChanged(server, false, _baseServerEventLogger);
                    server.PowerStatus = false;
                }
                else
                {
                    // means it was previously off
                    if (!server.PowerStatus)
                    {
                        await PowerStatusServerEventLogger.LogPowerServerStatusChanged(server, true, _baseServerEventLogger);
                    }
                    server.PowerStatus = true;
                }

                _unitOfWork.Servers.Update(server);
            }
        }
    }
    
}