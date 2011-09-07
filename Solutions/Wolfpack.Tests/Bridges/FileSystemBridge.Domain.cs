using System;
using Wolfpack.Core.Bridge.FileSystem;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Testing.Domains;

namespace Wolfpack.Tests.Bridges
{
    public class FileSystemBridgeDomainConfig
    {
        public string Folder { get; set; }
    }

    public class FileSystemBridgeDomain : MessengerEnabledDomain
    {
        protected FileSystemBridgeSessionPublisher mySessionPublisher;
        protected FileSystemBridgeResultPublisher myResultPublisher;
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
            mySessionPublisher = new FileSystemBridgeSessionPublisher(new FileSystemBridgePublisherConfig
                                                                  {
                                                                      Enabled = true,
                                                                      Folder = myConfig.Folder,
                                                                      FriendlyId = "AutomationFileSysPublisher"
                                                                  });
            myResultPublisher = new FileSystemBridgeResultPublisher(new FileSystemBridgePublisherConfig
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

        public void ThePublishersAreInvoked()
        {
            mySessionPublisher.Consume(new HealthCheckAgentStart
                                           {
                                             Id = Guid.NewGuid(),
                                             Agent  = new AgentInfo
                                                          {
                                                              AgentId = "AutomationAgent",
                                                              SiteId = "AutomationSite"
                                                          }
                                           });
            myResultPublisher.Consume(new HealthCheckResult
                                    {
                                        Id = Guid.NewGuid(),
                                        Check = new HealthCheckData
                                                    {
                                                        Identity = new PluginDescriptor
                                                                       {
                                                                           Name = "AutomationResultMessage"
                                                                       },
                                                        Result = true
                                                    }
                                    });
        }

        public void TheConsumerIsInvoked()
        {
            myConsumer.Execute();
        }
    }
}