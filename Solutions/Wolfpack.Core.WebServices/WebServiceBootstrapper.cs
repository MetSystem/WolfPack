using System;
using Magnum.Pipeline;
using ServiceStack.Razor;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces;
using Wolfpack.Core.WebServices.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces.Messages;
using Wolfpack.Core.WebServices.Strategies;
using Status = Wolfpack.Core.WebServices.Interfaces.Messages.Status;
using System.Linq;

namespace Wolfpack.Core.WebServices
{
    public class WebServiceBootstrapper : IStartupPlugin, ICanBeSwitchedOff 
    {
        public bool Enabled { get; set; }
        public Core.Interfaces.Entities.Status Status { get; set; }

        public WebServiceBootstrapper(WebServiceActivityConfig config)
        {
            Enabled = config.Enabled;
        }

        public void Initialise()
        {
            Container.RegisterAll<IWebServiceExtender>();            
            Container.RegisterAsSingleton<IActivityPlugin>(typeof (WebServiceActivity));

            if (!Container.IsRegistered<IWebServiceReceiverStrategy>())
            {
                Container.RegisterAll<IPipelineStep<WebServiceReceiverContext>>();
                Container.RegisterAsTransient<IWebServiceReceiverStrategy>(typeof (WebServiceReceiverStrategy));
            }
        }
    }

    public class RegisterCoreServices : IWebServiceExtender, IConsumer<NotificationEvent>
    {
        private WebServiceActivityConfig _config;

        public RegisterCoreServices(WebServiceActivityConfig config)
        {
            _config = config;
        }

        public void Add(IAppHost appHost)
        {
            Messenger.Subscribe(this);

            appHost.Routes.Add<Status>("status");
            appHost.Routes.Add<Activity>("activity");
            
            appHost.Routes.Add<NotificationEvent>("messages");
            appHost.Routes.Add<HealthCheckArtifact>("messages/{Name}/artifacts/{NotificationId}");

            appHost.Routes.Add<GetTagCloud>("configuration/tagcloud");
            appHost.Routes.Add<GetConfigurationCatalogue>("configuration/catalogue");
            appHost.Routes.Add<GetConfigurationCatalogue>("configuration/catalogue/{Tags}");
            appHost.Routes.Add<RestConfigurationChangeRequest>("configuration");
            appHost.Routes.Add<ApplyChanges>("configuration/applychanges");

            appHost.Routes.Add<Atom>("/feeds/atom");


            appHost.LoadPlugin(new RazorFormat(), new RequestLogsFeature());


            if (_config.ApiKeys != null && _config.ApiKeys.Any())
            {
                appHost.PreRequestFilters.AddIfNotExists(
                    (request, response) =>
                        {
                            var apikey = request.Headers["X-ApiKey"] ?? string.Empty;

                            if (!_config.ApiKeys.Contains(apikey, StringComparer.OrdinalIgnoreCase))
                                response.ReturnAuthRequired();
                        });
            }
        }

        public void Consume(NotificationEvent message)
        {
            if (message.EventType == NotificationEventAgentStart.EventTypeName)
            {
                var agentstart = Serialiser.FromJson<NotificationEventAgentStart>(message.Data);

                // possible to get multiple start messages - they could have been
                // backed up on the client so just keep overwriting this one.
                Container.RegisterInstance(agentstart, true);
            }
        }
    }
}