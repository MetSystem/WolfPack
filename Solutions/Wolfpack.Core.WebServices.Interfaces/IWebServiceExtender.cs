using ServiceStack.WebHost.Endpoints;

namespace Wolfpack.Core.WebServices.Interfaces
{
    public interface IWebServiceExtender
    {
        void Add(IAppHost appHost);
    }
}