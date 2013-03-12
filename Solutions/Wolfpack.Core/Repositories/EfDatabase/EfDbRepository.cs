using System;
using System.Linq;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Repositories.EfDatabase
{
    public class EfDbRepository : INotificationRepository
    {
        private readonly IEfDbContext _context;

        public EfDbRepository(IEfDbContext context)
        {
            _context = context;
        }

        public IQueryable<NotificationEvent> Query(params INotificationRepositoryQuery[] queries)
        {
            return queries.ToList().SelectMany(q => q.Query(_context.Notifications)).AsQueryable();
        }

        public void Add(NotificationEvent notification)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }

        public void Delete(Guid notificationId)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.Id.Equals(notificationId));

            if (notification == null)
                return;

            _context.Notifications.Remove(notification);
            _context.SaveChanges();
        }
    }
}