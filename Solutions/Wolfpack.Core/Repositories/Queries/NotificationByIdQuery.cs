using System;
using System.Linq;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Repositories.Queries
{
    public class NotificationByIdQuery : INotificationRepositoryQuery
    {
        private readonly Guid _id;

        public NotificationByIdQuery(Guid id)
        {
            _id = id;
        }

        public IQueryable<NotificationEvent> Query(IQueryable<NotificationEvent> data)
        {
            return data.Where(msg => msg.Id == _id);
        }
    }
}