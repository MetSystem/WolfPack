using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;
using Wolfpack.Core.Repositories.Sql;

namespace Wolfpack.Core.Publishers.Sql
{
    public class SqlPublisher : PublisherBase, INotificationEventPublisher
    {
        private readonly SqlPublisherConfiguration _config;

        private INotificationRepository _repository;

        public SqlPublisher(SqlPublisherConfiguration config)
        {
            _config = config;
            Enabled = config.Enabled;
            FriendlyId = config.FriendlyId;
        }

        public override void Initialise()
        {
            _repository = new SqlRepository(new SqlDbContext(_config.ConnectionName));
        }

        public void Consume(NotificationEvent message)
        {
            _repository.Add(message);
        }
    }
}