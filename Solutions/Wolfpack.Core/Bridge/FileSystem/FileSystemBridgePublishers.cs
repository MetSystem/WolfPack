using System.IO;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;
using Wolfpack.Core.Publishers;

namespace Wolfpack.Core.Bridge.FileSystem
{
    public class FileSystemBridgePublisherConfig : PluginConfigBase
    {
        public string Folder { get; set; }
    }

    public abstract class FileSystemBridgePublisherBase<T> : PublisherBase
    {
        protected readonly string myFolder;
        private readonly object mySyncLock = new object();

        protected FileSystemBridgePublisherBase(FileSystemBridgePublisherConfig config)
        {
            myFolder = SmartLocation.GetLocation(config.Folder);

            Enabled = config.Enabled;
            FriendlyId = config.FriendlyId; 
        }

        public void Publish(T message, string id)
        {
            // hmmm, performance?
            lock (mySyncLock)
            {                
                Directory.CreateDirectory(myFolder);
                var data = SerialisationHelper<T>.DataContractSerialize(message);
                var filename = string.Format("{0}.xml", id);
                var filepath = Path.Combine(myFolder, filename);

                using (var sw = new StreamWriter(filepath))
                {
                    sw.Write(data);
                }
            }
        }
    }

    public class FileSystemBridgeSessionPublisher : FileSystemBridgePublisherBase<HealthCheckAgentStart>, IHealthCheckSessionPublisher
    {

        public FileSystemBridgeSessionPublisher(FileSystemBridgePublisherConfig config)
            : base(config)
        {
        }

        public void Consume(HealthCheckAgentStart message)
        {
            Publish(message, message.Id.ToString());
        }
    }

    public class FileSystemBridgeResultPublisher : FileSystemBridgePublisherBase<HealthCheckResult>, IHealthCheckResultPublisher
    {

        public FileSystemBridgeResultPublisher(FileSystemBridgePublisherConfig config)
            : base(config)
        {
        }

        public void Consume(HealthCheckResult message)
        {
            Publish(message, message.Id.ToString());
        }
    }
}