using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Wolfpack.Core.WebServices.Interfaces;
using Wolfpack.Core.WebServices.Interfaces.Entities;
using Wolfpack.Core.WebServices.Modules;

namespace Wolfpack.Core.WebServices.Extenders
{
    public class CoreServicesExtender : IWebServiceExtender
    {
        private readonly WebServiceActivityConfig _config;

        public CoreServicesExtender(WebServiceActivityConfig config)
        {
            _config = config;
        }

        public IEnumerable<ModuleRegistration> Modules
        {
            get
            {
                return new []
                {
                    new ModuleRegistration(typeof(ConfigurationModule)), 
                    new ModuleRegistration(typeof(NotificationModule)), 
                };
            }
        }

        public void Execute(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.BeforeRequest += ctx =>
            {
                Logger.Debug("Received call to WebService: {0}", ctx.Request.Url);

                if (ApiKeysConfiguredButNonePresentedInRequest(ctx))
                {
                    Logger.Warning("Call failed, server is configured to expect an ApiKey from client requests!");
                    return new Response {StatusCode = HttpStatusCode.Unauthorized};
                }

                return null;
            };
        }

        private bool ApiKeysConfiguredButNonePresentedInRequest(NancyContext nancyContext)
        {
            if (_config.ApiKeys == null || !_config.ApiKeys.Any()) 
                return false;

            // TODO: filter UI requests from Api requests...only apply ApiKey check api calls??
            var apikey = nancyContext.Request.Headers["X-ApiKey"].FirstOrDefault() ?? string.Empty;
            return !_config.ApiKeys.Contains(apikey, StringComparer.OrdinalIgnoreCase);
        }
    }
}
