using System;
using System.Linq;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Interfaces
{
    public interface INotificationRepository
    {
        IQueryable<NotificationEvent> Query(params INotificationRepositoryQuery[] queries);
        void Add(NotificationEvent notification);
        void Delete(Guid notificationId);
    }
}