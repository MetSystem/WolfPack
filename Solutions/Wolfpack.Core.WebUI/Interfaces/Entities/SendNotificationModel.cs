using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.WebUI.Interfaces.Entities
{
    public class SendNotificationModel
    {
        public SendNotificationModel(NotificationEvent notification)
        {
            Notification = notification;
            Json = Serialiser.ToJson(notification, false);
        }

        public NotificationEvent Notification { get; private set; }
        public string Json { get; private set; }
    }
}