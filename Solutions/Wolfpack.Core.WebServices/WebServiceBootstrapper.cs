﻿using Wolfpack.Core.Interfaces;
using Wolfpack.Core.WebServices.Interfaces;
using Wolfpack.Core.WebServices.Interfaces.Entities;
using Wolfpack.Core.WebServices.Strategies;

namespace Wolfpack.Core.WebServices
{
    public class WebServiceBootstrapper : ISupportBootStrapping<WebServiceActivityConfig>
    {
        public void Execute(WebServiceActivityConfig config)
        {
            Container.RegisterInstance(new ActivityTracker());
            Container.RegisterAll<IWebServiceExtender>();

            if (!Container.IsRegistered<IWebServiceReceiverStrategy>())
            {
                Container.RegisterAll<IPipelineStep<WebServiceReceiverContext>>();
                Container.RegisterAsTransient<IWebServiceReceiverStrategy>(typeof(WebServiceReceiverStrategy));
            }
        }
    }
}