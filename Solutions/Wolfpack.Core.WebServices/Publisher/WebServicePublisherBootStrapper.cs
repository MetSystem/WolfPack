using Wolfpack.Core.Containers;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Repositories.FileSystem;
using Wolfpack.Core.WebServices.Client;
using Wolfpack.Core.WebServices.Interfaces;
using Wolfpack.Core.WebServices.Interfaces.Entities;
using Wolfpack.Core.WebServices.Strategies;
using Omu.ValueInjecter;

namespace Wolfpack.Core.WebServices.Publisher
{
    public class WebServicePublisherBootstrapper : IStartupPlugin, ICanBeSwitchedOff 
    {
        private readonly WebServicePublisherConfig _config;

        public WebServicePublisherBootstrapper(WebServicePublisherConfig config)
        {
            _config = config;
            Enabled = _config.Enabled;
        }

        public bool Enabled { get; set; }
        public Status Status { get; set; }

        public void Initialise()
        {
            Container.RegisterInstance(_config, If<WebServicePublisherConfig>.IsNotRegistered);

            if (!Container.IsRegistered<INotificationRepository>())
            {
                var repoConfig = new FileSystemNotificationRepositoryConfig
                                     {
                                         BaseFolder = _config.BaseFolder
                                     };
                Container.RegisterInstance(repoConfig);
                Container.RegisterAsTransient<INotificationRepository>(
                    typeof(FileSystemNotificationRepository));
            }

            if (!Container.IsRegistered<IWebServicePublisherStrategy>())
            {
                var clientConfig = new WolfpackWebServicesClientConfig();
                clientConfig.InjectFrom<LoopValueInjection>(_config);
                Container.RegisterInstance(clientConfig);

                Container.RegisterAsTransient<IWolfpackWebServicesClient>(typeof (WolfpackWebServicesClient));
                Container.RegisterAll<IPipelineStep<WebServicePublisherContext>>();
                Container.RegisterAsTransient<IWebServicePublisherStrategy>(
                    typeof(WebServicePublisherStrategy));
            }

            Container.RegisterAsSingleton<IActivityPlugin>(typeof (WebServicePublisher));
        }
    }
}