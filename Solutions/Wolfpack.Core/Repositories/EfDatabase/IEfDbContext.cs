using System.Data.Entity;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Repositories.EfDatabase
{
    public interface IEfDbContext
    {
        IDbSet<NotificationEvent> Notifications { get; set; }
        void SaveChanges();
    }
}