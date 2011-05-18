using System;
using Magnum;
using Wolfpack.Core.AppStats;
using Wolfpack.Core.Interfaces.Entities;
using NServiceBus;

namespace Wolfpack.Core.Bus
{
    public class AppStatNServiceBusPublisher : IAppStatsPublisher
    {
        private readonly string myDestinationQueue;
        private readonly IBus myBus;

        public AppStatNServiceBusPublisher(IBus bus)
        {
            myBus = bus;
        }

        public AppStatNServiceBusPublisher(IBus bus, string destinationQueue)
            : this(bus)
        {
            myDestinationQueue = destinationQueue;
        }

        public void Publish(AppStatsEvent stat)
        {
            Guard.AgainstNull(stat);
            Guard.AgainstEmpty(stat.CheckId);

            var result = new HealthCheckResultNotification
            {
                MinuteBucket = stat.MinuteBucket,
                HourBucket = stat.HourBucket,
                DayBucket = stat.DayBucket,
                EventType = "AppStat",
                Agent = new AgentInfo
                {
                    SiteId = stat.SiteId,
                    AgentId = stat.AgentId
                },
                Check = new HealthCheckData
                {
                    Duration = stat.Duration ?? TimeSpan.Zero,
                    Identity = new PluginDescriptor
                    {
                        Name = stat.CheckId
                    },
                    ResultCount = stat.ResultCount,
                    Tags = stat.Tag
                }
            };

            if (string.IsNullOrEmpty(myDestinationQueue))
                myBus.Send(result);
            else
                myBus.Send(myDestinationQueue, result);
        }
    }
}