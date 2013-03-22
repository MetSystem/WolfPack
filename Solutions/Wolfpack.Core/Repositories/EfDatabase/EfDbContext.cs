using System.Data.Entity;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Repositories.EfDatabase
{
    public class EfDbContext : DbContext, IEfDbContext
    {
        public IDbSet<NotificationEvent> Notifications { get; set; }

        public EfDbContext()
        {
        }

        public EfDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }
    }
}