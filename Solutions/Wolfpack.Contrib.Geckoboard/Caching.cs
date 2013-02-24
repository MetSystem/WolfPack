using System.Net;

namespace Wolfpack.Contrib.Geckoboard
{
    public class Caching
    {
        public static void NoCache()
        {
            //var responseProp = new HttpResponseMessageProperty();
            //responseProp.Headers[HttpResponseHeader.CacheControl] = "no-cache";
            //OperationContext.Current.OutgoingMessageProperties[HttpResponseMessageProperty.Name] =
            //    responseProp;           
        }
    }
}