using System;
using System.Net;
using RestSharp;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces;
using Wolfpack.Core.WebServices.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces.Exceptions;
using Wolfpack.Core.WebServices.Interfaces.Messages;
using System.Linq;

namespace Wolfpack.Core.WebServices.Client
{
    public class WolfpackWebServicesClient : IWolfpackWebServicesClient
    {
        private readonly WolfpackWebServicesClientConfig _config;
        private readonly RestClient _client;        

        public WolfpackWebServicesClient(WolfpackWebServicesClientConfig config)
        {
            _config = config;
            _client = new RestClient(config.BaseUrl);
        }

        public RestConfigurationCatalogue GetCatalogue(GetConfigurationCatalogue request)
        {
            var restRequest = new RestRequest("configuration/catalogue/{tags}");

            restRequest.AddUrlSegment("tags", string.Join(",", request.Tags ?? new string[0]));
            return ExecuteRequest<RestConfigurationCatalogue>(restRequest);
        }

        public ConfigurationCommandResponse Update(ConfigurationEntry request)
        {
            var restRequest = new RestRequest("configuration", Method.POST);
            restRequest.AddBody(request);
            return ExecuteRequest<ConfigurationCommandResponse>(restRequest);
        }

        public NotificationEventResponse Deliver(NotificationEvent notification)
        {
            var restRequest = new RestRequest("messages", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };

            restRequest.AddBody(notification);
            return ExecuteRequest<NotificationEventResponse>(restRequest);
        }

        public StatusResponse GetStatus()
        {
            var restRequest = new RestRequest("status", Method.GET);
            return ExecuteRequest<StatusResponse>(restRequest);
        }

        private T ExecuteRequest<T>(IRestRequest request)
        {
            const string apiKeyHeader = "X-ApiKey";

            if (!string.IsNullOrWhiteSpace(_config.ApiKey) &&
                !request.Parameters.Any(p => p.Type == ParameterType.HttpHeader &&
                    p.Name.Equals(apiKeyHeader, StringComparison.OrdinalIgnoreCase)))
            {
                request.AddHeader("X-ApiKey", _config.ApiKey);
            }

            var exec = typeof (RestClient).GetMethods()
                .Single(m => m.Name == "Execute" && m.IsGenericMethodDefinition)
                .MakeGenericMethod(typeof (T));
         
            var response = (IRestResponse<T>)exec.Invoke(_client, new object[] { request });
            
            AssertResponseStatus(response);
            return response.Data;
        }

        private static void AssertResponseStatus(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                var msg = string.Format("General failure calling {0}, status: {1}, reason: {2}",
                                        response.ResponseUri,
                                        response.ResponseStatus,
                                        response.ErrorMessage);
                throw new CommunicationException(msg);
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var msg = string.Format("Http failure calling {0}, status: {1}, reason: {2}",
                                        response.ResponseUri,
                                        response.StatusCode,
                                        response.StatusDescription);
                throw new CommunicationException(msg);
            }
        }
    }
}