using System;
using System.IO;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Castle.Core;
using Wolfpack.Core.Interfaces.Magnum;
using Wolfpack.Core.Wcf;

namespace Wolfpack.Core.AppStats.FileQueue
{
    public class AppStatsFileQueuePublishCheckConfig : WcfPublisherConfiguration
    {
        public string QueueFolder { get; set; }
    }

    public class AppStatsFileQueuePublishCheck : IHealthCheckPlugin
    {
        protected readonly IHealthCheckResultPublisher myPublisher;
        protected readonly AppStatsFileQueuePublishCheckConfig myConfig;
        protected PluginDescriptor  myIdentity;

        /// <summary>
        /// default ctor
        /// </summary>
        public AppStatsFileQueuePublishCheck(AppStatsFileQueuePublishCheckConfig config)
        {
            myConfig = config;
            myPublisher = new WcfResultPublisher(config);
            myIdentity = new PluginDescriptor
            {
                Description = string.Format("Publishes file queue from folder {0}", config.QueueFolder),
                TypeId = new Guid("EB2E1FF8-3E74-4400-BB09-CEF7E597C3EB"),
                Name = myConfig.FriendlyId
            };
        }

        public Status Status { get; set; }

        public PluginDescriptor Identity
        {
            get { return myIdentity; }
        }
       
        public void Initialise()
        {
            // do nothing
        }

        public void Execute()
        {
            Directory.GetFiles(myConfig.QueueFolder).ForEach(filename =>
                                                                 {
                                                                     string data;

                                                                     using (var sr = new StreamReader(filename))
                                                                     {
                                                                         data = sr.ReadToEnd();
                                                                     }

                                                                     var result =
                                                                         SerialisationHelper<HealthCheckResult>.
                                                                             DataContractDeserialize(data);

                                                                     myPublisher.Consume(result);                                                                     
                                                                     File.Delete(filename);
                                                                 });
        }
    }
}