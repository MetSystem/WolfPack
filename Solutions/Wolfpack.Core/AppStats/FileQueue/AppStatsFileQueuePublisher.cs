using System.IO;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.AppStats.FileQueue
{
    public class AppStatsFileQueuePublisher : AppStatsPublisherBase
    {
        private readonly string myFolder;
        private readonly object mySyncLock = new object();

        public AppStatsFileQueuePublisher(string folder)
        {
            myFolder = folder;
        }

        public override void Publish(AppStatsEvent stat)
        {
            // hmmm, performance?
            lock (mySyncLock)
            {
                var result = new HealthCheckResult();
                DefaultMap(stat, result);

                Directory.CreateDirectory(myFolder);
                var data = SerialisationHelper<HealthCheckResult>.DataContractSerialize(result);
                var filename = string.Format("{0}.xml", result.Id);
                var filepath = Path.Combine(myFolder, filename);

                using (var sw = new StreamWriter(filepath))
                {
                    sw.Write(data);
                }
            }
        }
    }
}