using Wolfpack.Core.AppStats;
using NServiceBus;

namespace Wolfpack.Core.Bus
{
    public class AppStatNServiceBusPublisher : AppStatsPublisherBase
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

        public override void Publish(AppStatsEvent stat)
        {
            var result = new HealthCheckResultNotification();
            DefaultMap(stat, result);

            if (string.IsNullOrEmpty(myDestinationQueue))
                myBus.Send(result);
            else
                myBus.Send(myDestinationQueue, result);
        }
    }
}