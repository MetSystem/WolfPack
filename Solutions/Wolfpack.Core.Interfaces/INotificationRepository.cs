using System;
using System.Linq;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Interfaces
{
    public interface INotificationRepository
    {
        IQueryable<NotificationEvent> Query(params INotificationRepositoryQuery[] queries);
        void Save<T>(T message) where T: NotificationEvent;
        void Delete(Guid notificationId);
    }
}