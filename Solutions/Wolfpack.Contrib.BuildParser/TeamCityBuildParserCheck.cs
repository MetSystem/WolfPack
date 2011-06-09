using System;
using Sharp2City;
using Wolfpack.Core.Checks;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Contrib.BuildParser
{
    public class BuildParserCheckConfig : PluginConfigBase
    {
        public string NCoverHtmlSummaryResultsFile { get; set; }
    }

    public class TeamCityBuildParserCheck : HealthCheckBase
    {
        protected BuildParserCheckConfig myConfig;

        /// <summary>
        /// default ctor
        /// </summary>
        public TeamCityBuildParserCheck(BuildParserCheckConfig config)
        {
            myConfig = config;
        }

        public override void Initialise()
        {
        }

        public override void Execute()
        {
            Publish(new HealthCheckData
                        {
                            Identity = Identity
                        });
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
                       {
                           Description = "Parses build output",
                           TypeId = new Guid("040CF1F2-7000-4902-86EA-7E348E2EEC2C"),
                           Name = myConfig.FriendlyId
                       };
        }
    }
}