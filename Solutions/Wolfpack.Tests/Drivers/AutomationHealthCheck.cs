using System;
using System.Threading;
using Wolfpack.Core.Checks;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Tests.Checks;
using Castle.Core;

namespace Wolfpack.Tests.Drivers
{
    public class AutomationHealthCheck : StreamThresholdCheckBase<StreamThresholdBasedDomainConfig>
    {
        public AutomationHealthCheck(StreamThresholdBasedDomainConfig config)
            : base(config)
        {
        }

        public override void Execute()
        {
            for (var i = 0; i < myConfig.Cycles; i++)
            {
                myConfig.Values.ForEach(rv =>
                                            {
                                                Publish(new HealthCheckData
                                                            {
                                                                Identity = Identity,
                                                                ResultCount = rv

                                                            });

                                                Thread.Sleep(myConfig.Interval);
                                            });
                
            }
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
                       {
                           Description = "Test Automation HealthCheck",
                           Name = "TestAutomationHealthCheck",
                           TypeId = new Guid("17FFDF11-88A9-4095-A43C-111F2F7EC3AB")
                       };
        }
    }
}