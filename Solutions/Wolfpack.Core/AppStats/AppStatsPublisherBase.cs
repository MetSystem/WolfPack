using System;
using Magnum;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.AppStats
{
    public abstract class AppStatsPublisherBase : IAppStatsPublisher
    {
        protected void DefaultMap(AppStatsEvent stat, HealthCheckResult result)
        {
            Guard.AgainstNull(stat);
            Guard.AgainstEmpty(stat.CheckId);

            result.MinuteBucket = stat.MinuteBucket;
            result.HourBucket = stat.HourBucket;
            result.DayBucket = stat.DayBucket;
            result.EventType = "AppStat";
            result.Agent = new AgentInfo
                        {
                            SiteId = stat.SiteId,
                            AgentId = stat.AgentId
                        };
            result.Check = new HealthCheckData
                        {
                            Duration = stat.Duration ?? TimeSpan.Zero,
                            Identity = new PluginDescriptor
                                           {
                                               Name = stat.CheckId
                                           },
                            ResultCount = stat.ResultCount,
                            Tags = stat.Tag
                        };
        }

        public abstract void Publish(AppStatsEvent stat);
    }
}