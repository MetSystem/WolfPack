using ServiceStack.ServiceHost;
using Wolfpack.Core.WebServices.Interfaces.Messages;

namespace Wolfpack.Core.WebServices.Services
{
    public class FeedServices : IService<Atom>
    {
        public object Execute(Atom request)
        {
            return new AtomResponse();
        }
    }
}