using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using ServerSightAPI.Models;

namespace ServerSightAPI.Notifications
{
    public class Notification
    {
        /*
         * Sends notifications with firebase.
         * @param devicesToSendNotificationTo - Needs to be provided by a client device (web, app)
         */
        public static async Task SendNotification(string title, string description, IList<FirebaseDevice> devicesToSendNotificationTo)
        {
            IEnumerable<string> deviceKeys = devicesToSendNotificationTo.Select(d => d.DeviceKey);
            var message = new MulticastMessage()  
            {  
                Tokens = new List<string>(deviceKeys),  
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = title,
                    Body = description
                },  
            };
            await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).ConfigureAwait(true);
        }
    }
}