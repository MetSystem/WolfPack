using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces.Messages;
using System.Linq;

namespace Wolfpack.Core.WebServices.Services
{
    public class ConfigurationServices : Service
    {
        private readonly WebServiceActivityConfig _config;

        public ConfigurationServices(WebServiceActivityConfig config)
        {
            _config = config;
        }

        public List<TagCloudEntry> Get(GetTagCloud request)
        {
            return ConfigurationManager.GetTagCloud().ToList();
        }

        public object Get(GetConfigurationCatalogue request)
        {
            var catalogue = ConfigurationManager.GetCatalogue(request.Tags);
            var response = new RestConfigurationCatalogue
                               {
                                   Links = new List<RestLink>
                                               {
                                                    new RestLink
                                                    { 
                                                        Action = "Accept Changes", 
                                                        Link = BuildLink("configuration/applychanges"), 
                                                        Method = "GET" 
                                                    }
                                               },
                                   Pending = catalogue.Pending.Select(
                                       ce =>
                                           {
                                               var entry = new RestConfigurationChangeSummary(ce);
                                               entry.Links.Add(new RestLink
                                                                   {
                                                                       Action = "Undo change",
                                                                       Method = "GET",
                                                                       Link = BuildLink("configuration/undo?id={0}", ce.Id)
                                                                   });
                                               return entry;
                                           }
                                       ).ToList(),
                                   Items = catalogue.Items.Select(
                                       ce =>
                                           {
                                               var entry = new RestCatalogueEntry(ce);
                                               entry.Links.Add(new RestLink
                                                                   {
                                                                       Action = "Create a new instance",
                                                                       Method = "POST",
                                                                       Link = BuildLink("configuration")
                                                                   });
                                               return entry;
                                           }).ToList()
                               };
            return response;
        }

        public object Post(RestConfigurationChangeRequest request)
        {
            try
            {
                ConfigurationManager.Update(request.ToChangeRequest());
                return new ConfigurationCommandResponse { Result = true };
            }
            catch (Exception e)
            {
                return new ConfigurationCommandResponse { Result = false, Error = e };
            }
        }

        public object Get(ApplyChanges request)
        {
            try
            {
                ConfigurationManager.ApplyPendingChanges(request.Restart);
                return new ConfigurationCommandResponse { Result = true };
            }
            catch (Exception e)
            {
                return new ConfigurationCommandResponse { Result = false, Error = e };
            }            
        }

        private string BuildLink(string relativePathTemplate, params object[] args)
        {
            var relativePath = string.Format(relativePathTemplate, args).TrimStart('/');
            return string.Format("{0}/{1}", _config.BaseUrl.TrimEnd('/'), relativePath);
        }
    }
}