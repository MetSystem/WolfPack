using Wolfpack.Core.Interfaces;
using Wolfpack.Core.WebServices.Interfaces;
using Wolfpack.Core.WebServices.Interfaces.Entities;
using Wolfpack.Core.WebServices.Strategies;
using Wolfpack.Core.WebServices.Strategies.Steps;

namespace Wolfpack.Core.WebServices
{
    public class WebServiceBootstrapper : ISupportBootStrapping<WebServiceActivityConfig>
    {
        public void Execute(WebServiceActivityConfig config)
        {
            var tracker = new ActivityTracker();
            Container.RegisterInstance(tracker);

            Container.RegisterAll<IWebServiceExtender>();

            if (!Container.IsRegistered<IWebServiceReceiverStrategy>())
            {
                if (!Container.IsRegistered<MessageStalenessCheckConfig>())
                {
                    Container.RegisterAsSingleton(typeof(MessageStalenessCheckConfig));
                }

                Container.RegisterAll<IPipelineStep<WebServiceReceiverContext>>();
                Container.RegisterAsTransient<IWebServiceReceiverStrategy>(typeof(WebServiceReceiverStrategy));
            }
        }
    }
}