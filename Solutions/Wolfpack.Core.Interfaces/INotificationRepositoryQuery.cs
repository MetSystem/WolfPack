using System.Linq;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Interfaces
{
    public interface INotificationRepositoryQuery
    {
        IQueryable<NotificationEvent> Query(IQueryable<NotificationEvent> data);
    }
}