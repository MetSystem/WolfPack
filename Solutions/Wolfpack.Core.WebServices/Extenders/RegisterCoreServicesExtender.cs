using System;
using System.Linq;
using Magnum.Pipeline;
using ServiceStack;
using ServiceStack.Razor;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces;
using Wolfpack.Core.WebServices.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces.Messages;
using Status = Wolfpack.Core.WebServices.Interfaces.Messages.Status;

namespace Wolfpack.Core.WebServices.Extenders
{
    public class RegisterCoreServicesExtender : IWebServiceExtender, IConsumer<NotificationEvent>
    {
        private readonly WebServiceActivityConfig _config;

        public RegisterCoreServicesExtender(WebServiceActivityConfig config)
        {
            _config = config;
        }

        public void Add(IAppHost appHost)
        {
            Messenger.Subscribe(this);
            
            appHost.Plugins.Add(new RazorFormat());
            appHost.Plugins.Add(new RequestLogsFeature());

            //appHost.Routes.Add<Status>("/status");
            appHost.Routes.Add<Activity>("/activity");
            appHost.Routes.Add<NotificationEvent>("/messages");
            appHost.Routes.Add<HealthCheckArtifact>("/messages/{Name}/artifacts/{NotificationId}");
            //appHost.Routes.Add<GetTagCloud>("/configuration/tagcloud");
            //appHost.Routes.Add<GetConfigurationCatalogue>("/configuration/catalogue");
            //appHost.Routes.Add<GetConfigurationCatalogue>("/configuration/catalogue/{Tags}");
            //appHost.Routes.Add<RestConfigurationChangeRequest>("/configuration");
            //appHost.Routes.Add<ApplyChanges>("/configuration/applychanges");
            //appHost.Routes.Add<Atom>("/feeds/atom");

            if (_config.ApiKeys != null && _config.ApiKeys.Any())
            {
                appHost.PreRequestFilters.AddIfNotExists(
                    (request, response) =>
                        {
                            var apikey = request.Headers["X-ApiKey"] ?? string.Empty;

                            if (!_config.ApiKeys.Contains(apikey, StringComparer.OrdinalIgnoreCase))
                                HttpResponseExtensions.ReturnAuthRequired(response);
                        });
            }
        }

        public void Consume(NotificationEvent notification)
        {            
            Container.Resolve<ActivityTracker>().Track(notification);
            Logger.Debug("Tracking notification {0}", notification.Id);
        }
    }
}