using System.ServiceModel;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;
using Wolfpack.Core.Publishers;

namespace Wolfpack.Core.Wcf
{
    public class WcfSessionPublisher : PublisherBase, IHealthCheckSessionPublisher
    {        
        protected readonly WcfPublisherConfiguration myConfig;
        protected ChannelFactory<IWolfpack> myFactory;

        public WcfSessionPublisher(WcfPublisherConfiguration config)
        {
            myConfig = config;

            Enabled = myConfig.Enabled;
            FriendlyId = myConfig.FriendlyId;
        }

        public void Publish(HealthCheckAgentStart message)
        {
            IWolfpack proxy = null;

            try
            {
                if (myFactory == null)
                {
                    myFactory = new ChannelFactory<IWolfpack>(
                        new BasicHttpBinding(),
                        myConfig.Uri
                        );
                }

                proxy = myFactory.CreateChannel();
                proxy.CaptureAgentStart(message);
                ((IClientChannel)proxy).Close();
            }
            catch
            {
                if ((proxy != null) && (((IClientChannel)proxy).State == CommunicationState.Faulted))
                    ((IClientChannel)proxy).Abort();

                throw;
            }
        }

        public void Consume(HealthCheckAgentStart message)
        {
            Publish(message);
        }
    }

    public class WcfResultPublisher : PublisherBase, IHealthCheckResultPublisher
    {
        protected ChannelFactory<IWolfpack> myFactory;
        protected readonly WcfPublisherConfiguration myConfig;

        public WcfResultPublisher(WcfPublisherConfiguration config)
        {
            myConfig = config;

            Enabled = myConfig.Enabled;
            FriendlyId = myConfig.FriendlyId;
        }

        public void Publish(HealthCheckResult message)        
        {
            IWolfpack proxy = null;

            try
            {
                if (myFactory == null)
                {
                    myFactory = new ChannelFactory<IWolfpack>(
                        new BasicHttpBinding(),
                        myConfig.Uri);
                }

                proxy = myFactory.CreateChannel();
                proxy.CaptureResult(message);
                ((IClientChannel)proxy).Close();
            }
            catch
            {
                if ((proxy != null) && (((IClientChannel)proxy).State == CommunicationState.Faulted))
                    ((IClientChannel)proxy).Abort();

                throw;
            }                        
        }

        public void Consume(HealthCheckResult message)
        {
            Publish(message);
        }
    }
}