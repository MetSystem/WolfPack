using System.IO;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Publishers;

namespace Wolfpack.Core.Bridge.FileSystem
{
    public class FileSystemBridgePublisher : PublisherBase
    {
        private readonly string myFolder;
        private readonly object mySyncLock = new object();

        public FileSystemBridgePublisher(string folder)
        {
            myFolder = folder;
        }

        public void Publish(HealthCheckResult message)
        {
            // hmmm, performance?
            lock (mySyncLock)
            {
                Directory.CreateDirectory(myFolder);
                var data = SerialisationHelper<HealthCheckResult>.DataContractSerialize(message);
                var filename = string.Format("{0}.xml", message.Id);
                var filepath = Path.Combine(myFolder, filename);

                using (var sw = new StreamWriter(filepath))
                {
                    sw.Write(data);
                }
            }
        }
    }
}