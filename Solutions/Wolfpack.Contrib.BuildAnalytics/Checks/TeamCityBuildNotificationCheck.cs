using System;
using System.Globalization;
using Sharp2City;
using Wolfpack.Core;
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
            Logger.Debug("TeamCityBuildNotificationCheck inspecting {0}/{1}", _config.ProjectName, _config.ConfigurationName);
            var tc = new TeamCityClient(_config.BuildServerUrl,
                                        _config.UserId, _config.Password,
                                        _config.UseSsl, _config.TrustDuffSslCertificate);
            var build = tc.GetLastBuild(tc.FindBuildConfiguration(_config.ProjectName, _config.ConfigurationName));

            if (build.BuildId == myLastBuildId)
                return;

            // new build, publish it
            myLastBuildId = build.BuildId;
            var buildResult = (build.BuildStatus == BuildStatus.Success);

            Logger.Debug("\tBuild {0} for '{1}/{2}' has been detected (Success:={3})", 
                myLastBuildId, 
                _config.ProjectName, _config.ConfigurationName,
                buildResult);
            
            var duration = build.FinishDate.Subtract(build.StartDate);

            var data = HealthCheckData.For(Identity,
                                           "Status of build '{0}' for project '{1}: {2}",
                                           _config.ConfigurationName, _config.ProjectName,
                                           build.StatusText)
                .ResultIs(buildResult)
                .ResultCountIs(duration.TotalSeconds)
                .SetDuration(duration)
                .SetGeneratedOnUtc(build.FinishDate)
                .AddTag(build.BuildId.ToString(CultureInfo.InvariantCulture));

            Publish(NotificationRequest.AlwaysPublish(data, cfg => cfg.DataKeyGenerator = MakeUniqueNotificationKey));
        }

        private string MakeUniqueNotificationKey(HealthCheckData dataToPublish)
        {
            throw new NotImplementedException();
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
                       {
                           Description = "Detects new TeamCity builds and reports on their state",
                           TypeId = new Guid("040CF1F2-7000-4902-86EA-7E348E2EEC2C"),
                           Name = _config.FriendlyId
                       };
        }
    }
}