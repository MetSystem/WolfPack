using System;
using System.IO;
using System.Linq;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Repositories.FileSystem
{
    public class FileSystemNotificationRepository : INotificationRepository
    {
        private readonly FileSystemNotificationRepositoryConfig _config;

        public FileSystemNotificationRepository(FileSystemNotificationRepositoryConfig config)
        {
            _config = config;
        }

        public void Save<T>(T message) where T : NotificationEvent
        {
            var filename = MakeItemFilename(message.Id);

            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            Serialiser.ToJsonInFile(filename, message);

            Logger.Debug("Stored Notification ({0}): {1}", message.EventType, filename);
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

        public IQueryable<NotificationEvent> Query(params INotificationRepositoryQuery[] queries)
        {
            var messages = LoadMessages();
            return queries.SelectMany(q => q.Query(messages)).AsQueryable();
        }

        private IQueryable<NotificationEvent> LoadMessages()
        {
            return Directory.GetFiles(SmartLocation.GetLocation(_config.BaseFolder), "*.*", SearchOption.TopDirectoryOnly)
                .ToList().Select(f => Serialiser.FromJsonInFile<NotificationEvent>(f)).AsQueryable();
        }
    }
}