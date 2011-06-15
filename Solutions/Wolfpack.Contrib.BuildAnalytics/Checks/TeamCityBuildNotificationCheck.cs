using System;
using Sharp2City;
using Wolfpack.Core.Checks;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Contrib.BuildAnalytics.Checks
{
    public class TeamCityBuildNotificationCheckConfig : PluginConfigBase
    {
        public string BuildServerUrl { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
        public bool TrustDuffSslCertificate { get; set; }
        public string ProjectName { get; set; }
        public string ConfigurationName { get; set; }
    }

    public class TeamCityBuildNotificationCheck : HealthCheckBase<TeamCityBuildNotificationCheckConfig>
    {
        private int myLastBuildId;

        /// <summary>
        /// default ctor
        /// </summary>
        public TeamCityBuildNotificationCheck(TeamCityBuildNotificationCheckConfig config)
            : base(config)
        {
        }

        public override void Initialise()
        {
        }

        public override void Execute()
        {
            // use sharp2city here...
            var tc = new TeamCityClient(myConfig.BuildServerUrl,
                                        myConfig.UserId, myConfig.Password,
                                        myConfig.UseSsl, myConfig.TrustDuffSslCertificate);
            var build = tc.GetLastBuild(tc.FindBuildConfiguration(myConfig.ProjectName, myConfig.ConfigurationName));

            if (build.BuildId == myLastBuildId)
                return;

            // new build, publish it
            myLastBuildId = build.BuildId;
            var duration = build.FinishDate.Subtract(build.StartDate);

            Publish(new HealthCheckData
                        {
                            Identity = Identity,
                            Duration = duration,
                            Result = (build.BuildStatus == BuildStatus.Success),
                            ResultCount = duration.TotalMinutes,
                            Info = string.Format("Status of build '{0}' for project '{1}: {2}",
                                                 myConfig.ConfigurationName, myConfig.ProjectName, build.StatusText),
                            Tags = build.BuildId.ToString()
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