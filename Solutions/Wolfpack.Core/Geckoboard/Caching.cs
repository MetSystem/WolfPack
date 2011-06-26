using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Wolfpack.Core.Geckoboard
{
    public class Caching
    {
        public static void NoCache()
        {
            var responseProp = new HttpResponseMessageProperty();
            responseProp.Headers[HttpResponseHeader.CacheControl] = "no-cache";
            OperationContext.Current.OutgoingMessageProperties[HttpResponseMessageProperty.Name] =
                responseProp;           
        }
    }
}