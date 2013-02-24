using System;
using System.Collections.Generic;
using ServiceStack.ServiceHost;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.WebServices;
using IPlugin = ServiceStack.WebHost.Endpoints.IPlugin;

namespace Wolfpack.Contrib.Geckoboard
{

    public class GeckoboardDataServiceActivityConfig : ConfigBase
    {
        public string ServiceImplementation { get; set; }
        public string Uri { get; set; }
        public string ApiKey { get; set; }
    }

    /// <summary>
    /// Provides a WCF REST ServiceHost suitable for hosting within a windows service
    /// </summary>
    public class GeckoboardDataServices : IWebServiceExtender
    {
        protected readonly GeckoboardDataServiceActivityConfig _config;

        /// <summary>
        /// default ctor
        /// </summary>
        /// <param name="config"></param>
        public GeckoboardDataServices(GeckoboardDataServiceActivityConfig config)
        {
            _config = config;
        }

        public void Add(Funq.Container container, IServiceRoutes routes, List<IPlugin> plugins)
        {
            throw new NotImplementedException();
        }
    }

    //public class GeckoboardApiKeyInterceptor : RequestInterceptor
    //{
    //    protected readonly string myApiKey;

    //    public GeckoboardApiKeyInterceptor(string apiKey) 
    //        : base(false)
    //    {
    //        myApiKey = apiKey;
    //    }

    //    public override void ProcessRequest(ref RequestContext requestContext)
    //    {
    //        if (!IsValidApiKey(requestContext))
    //            GenerateErrorResponse(requestContext, HttpStatusCode.Unauthorized,
    //                "Missing or invalid ApiKey");
    //    }

    //    public bool IsValidApiKey(RequestContext requestContext)
    //    {
    //        var request = requestContext.RequestMessage;
    //        var requestProp = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
    //        var apikey = requestProp.Headers["Authorization"];
    //        var b64Key = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(myApiKey + ":X"));
    //        return (string.CompareOrdinal(b64Key, apikey) == 0);
    //    }

    //    public void GenerateErrorResponse(RequestContext requestContext,
    //        HttpStatusCode statusCode, string errorMessage)
    //    {
    //        // The error message is padded so that IE shows the response by default
    //        var errorXml =
    //            "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root><error>Access denied</error></root>";
    //        var response = XElement.Load(new StringReader(string.Format(errorXml, errorMessage)));
    //        var reply = Message.CreateMessage(MessageVersion.None, null, response);
    //        var responseProp = new HttpResponseMessageProperty
    //                               {
    //                                   StatusCode = statusCode
    //                               };
    //        responseProp.Headers[HttpResponseHeader.ContentType] = "text/xml";
    //        reply.Properties[HttpResponseMessageProperty.Name] = responseProp;
    //        requestContext.Reply(reply);

    //        // set the request context to null to terminate processing of this request
    //        requestContext = null;
    //    }
    //}
}