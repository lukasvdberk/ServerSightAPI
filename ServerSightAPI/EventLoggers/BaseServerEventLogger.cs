using System;
using System.Threading.Tasks;
using Microsoft.OpenApi.Extensions;
using ServerSightAPI.Models.Server;
using ServerSightAPI.Notifications;
using ServerSightAPI.Repository;

namespace ServerSightAPI.EventLoggers
{
    public class BaseServerEventLogger : IBaseServerEventLogger
    {
        private IUnitOfWork _unitOfWork;
        
        public BaseServerEventLogger(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task LogEvent(string description, EventType eventType, Server server)
        {
            await SaveServerEventInDatabase(description, eventType, server);
            
            // TODO set notification pushing in background
            await SendPushNotificationsToClients(description, eventType, server);
        }

        private async Task SendPushNotificationsToClients(string description, EventType eventType, Server server)
        {
            var devices =  await _unitOfWork.FirebaseDevices.GetAll(q => q.OwnedById == server.OwnedById);

            string notificationTitle = eventType.GetDisplayName();
            Notification.SendNotification(notificationTitle, description, devices);
        }
        
        private  async Task SaveServerEventInDatabase(string description, EventType eventType, Server server)
        {
            var serverPowerEvent = new ServerEvent
            {
                CreatedAt = DateTime.Now,
                EventType = eventType,
                Description = description,
                ServerId = server.Id
            };
            await _unitOfWork.ServerEvents.Insert(serverPowerEvent);
        }
    }
}