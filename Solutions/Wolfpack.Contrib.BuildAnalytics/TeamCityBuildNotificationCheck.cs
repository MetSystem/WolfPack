using System;
using Wolfpack.Core.Checks;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Contrib.BuildAnalytics
{
    public class TeamCityBuildNotificationCheckConfig : PluginConfigBase
    {
        public string Server { get; set; }
    }

    public class TeamCityBuildNotificationCheck : HealthCheckBase
    {
        protected TeamCityBuildNotificationCheckConfig myConfig;

        /// <summary>
        /// default ctor
        /// </summary>
        public TeamCityBuildNotificationCheck(TeamCityBuildNotificationCheckConfig config)
        {
            myConfig = config;
        }

        public override void Initialise()
        {
        }

        public override void Execute()
        {
            // use sharp2city here...

            Publish(new HealthCheckData
                        {
                            Identity = Identity
                        });
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
                       {
                           Description = "Detects new TeamCity builds and reports on their state",
                           TypeId = new Guid("040CF1F2-7000-4902-86EA-7E348E2EEC2C"),
                           Name = myConfig.FriendlyId
                       };
        }
    }
}