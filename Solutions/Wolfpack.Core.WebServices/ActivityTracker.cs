using System.Collections.Concurrent;
using System.Collections.Generic;
using Wolfpack.Core.Interfaces.Entities;
using System.Linq;

namespace Wolfpack.Core.WebServices
{
    public class ActivityTracker
    {
        private readonly ConcurrentQueue<NotificationEvent> _notifications;

        public NotificationEventAgentStart StartEvent { get; private set; }

        public IEnumerable<NotificationEvent> Notifications { get { return _notifications.ToArray().OrderByDescending(n => n.GeneratedOnUtc); } }

        public ActivityTracker()
        {
            _notifications = new ConcurrentQueue<NotificationEvent>();
        }

        public void Track(NotificationEvent notification)
        {
            if (notification.EventType == NotificationEventAgentStart.EventTypeName)
            {
                StartEvent = Serialiser.FromJson<NotificationEventAgentStart>(notification.Data);
            }
            else
            {
                _notifications.Enqueue(notification);

                while (_notifications.Count > 20)
                {
                    NotificationEvent item;
                    _notifications.TryDequeue(out item);
                }
            }
        }
    }
}