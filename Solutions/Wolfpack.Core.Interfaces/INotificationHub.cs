using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Interfaces
{
    public interface INotificationHub
    {
        void Initialise(AgentInfo info);
        INotificationRequestFilter SelectFilter(NotificationRequest request);
        NotificationEvent ConvertRequestToEvent(NotificationRequest request);
        void PublishEvent(NotificationEvent notification);
    }
}