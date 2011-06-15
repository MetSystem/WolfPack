using System;
using Wolfpack.Contrib.BuildAnalytics.Parsers;
using Wolfpack.Core.Bridge.FileSystem;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Tests.Bdd;

namespace Wolfpack.Tests.Bridges
{
    public class FileSystemBridgeDomainConfig
    {
        public string Folder { get; set; }
    }

    public class FileSystemBridgeDomain : MessengerEnabledDomain
    {
        protected FileSystemBridgeResultPublisher myPublisher;
        protected FileSystemBridgeCheck myConsumer;
        private readonly FileSystemBridgeDomainConfig myConfig;

        public FileSystemBridgeDomain(FileSystemBridgeDomainConfig config)
        {
            myConfig = config;
        }

        public override void Dispose()
        {
            
        }

        public void TheBridgeComponents()
        {
            myPublisher = new FileSystemBridgeResultPublisher(new FileSystemBridgePublisherConfig
                                                                  {
                                                                      Enabled = true,
                                                                      Folder = myConfig.Folder,
                                                                      FriendlyId = "AutomationFileSysPublisher"
                                                                  });
            myConsumer = new FileSystemBridgeCheck(new FileSystemBridgeCheckConfig
                                                       {
                                                           Enabled = true,
                                                           Folder = myConfig.Folder,
                                                           FriendlyId = "AutomationFileSysConsumer"
                                                       });
        }

        public void ThePublisherIsInvoked()
        {
            myPublisher.Publish(new HealthCheckResult
                                    {
                                        Check = new HealthCheckData
                                                    {
                                                        Identity = new PluginDescriptor
                                                                       {
                                                                           Name = "AutomationResultMessage"
                                                                       },
                                                        Result = true
                                                    }
                                    },
                                Guid.NewGuid().ToString());
        }

        public void TheConsumerIsInvoked()
        {
            myConsumer.Execute();
        }
    }
}