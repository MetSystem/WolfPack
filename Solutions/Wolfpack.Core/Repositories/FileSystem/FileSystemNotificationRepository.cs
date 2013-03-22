using System;
using System.IO;
using System.Linq;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Castle.Core.Internal;

namespace Wolfpack.Core.Repositories.FileSystem
{
    public class FileSystemNotificationRepository : INotificationRepository
    {
        private readonly FileSystemNotificationRepositoryConfig _config;

        public FileSystemNotificationRepository(FileSystemNotificationRepositoryConfig config)
        {
            _config = config;
        }

        public bool GetById(Guid id, out NotificationEvent notification)
        {
            notification = LoadAll().FirstOrDefault(n => n.Id.Equals(id));
            return notification != null;
        }

        public IQueryable<NotificationEvent> GetByState(MessageStateTypes state)
        {
            return LoadAll().Where(n => n.State.Equals(state));
        }

        public void Add(NotificationEvent notification)
        {
            var filename = MakeItemFilename(notification.Id);

            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            Serialiser.ToJsonInFile(filename, notification);

            Logger.Debug("Stored Notification ({0}): {1}", notification.EventType, filename);
        }

        private string MakeItemFilename(Guid id)
        {
            return Path.Combine(SmartLocation.GetLocation(_config.BaseFolder),
                Path.ChangeExtension(id.ToString(), "txt"));
        }

        public void Delete(Guid notificationId)
        {
            var filename = MakeItemFilename(notificationId);
            File.Delete(filename);
        }

        public IQueryable<NotificationEvent> Filter(params INotificationRepositoryQuery[] filters)
        {
            var result = LoadAll();
            return filters.Aggregate(result, (current, filter) => filter.Filter(current));
        }

        private IQueryable<NotificationEvent> LoadAll()
        {
            return Directory.GetFiles(SmartLocation.GetLocation(_config.BaseFolder), "*.*", SearchOption.TopDirectoryOnly)
                .ToList().Select(f => Serialiser.FromJsonInFile<NotificationEvent>(f)).AsQueryable();
        }
    }
}