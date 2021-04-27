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
        public async Task LogEvent(string eventName,string description, EventType eventType, Server server)
        {
            await SaveServerEventInDatabase(description, eventType, server);
            await SendPushNotificationsToClients(eventName,description, eventType, server);
        }

        private async Task SendPushNotificationsToClients(string title, string description, EventType eventType, Server server)
        {
            var devices =  await _unitOfWork.FirebaseDevices.GetAll(q => q.OwnedById == server.OwnedById);

            await Notification.SendNotification(title, description, devices);
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