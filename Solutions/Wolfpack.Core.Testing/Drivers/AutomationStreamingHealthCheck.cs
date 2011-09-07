using System;
using System.Threading;
using Wolfpack.Core.Checks;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Testing.Domains;
using System.Linq;

namespace Wolfpack.Core.Testing.Drivers
{
    public class AutomationStreamingHealthCheck : StreamThresholdCheckBase<StreamingHealthCheckDomainConfig>
    {
        public AutomationStreamingHealthCheck(StreamingHealthCheckDomainConfig config)
            : base(config)
        {
        }

        public override void Execute()
        {
            for (var i = 0; i < myConfig.Cycles; i++)
            {
                myConfig.Values.ToList().ForEach(rv =>
                                            {
                                                Publish(new HealthCheckData
                                                            {
                                                                Identity = Identity,
                                                                ResultCount = rv

                                                            });

                                                Thread.Sleep((int) myConfig.Interval);
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