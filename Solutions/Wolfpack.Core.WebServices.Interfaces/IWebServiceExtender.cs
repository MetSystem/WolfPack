
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace Wolfpack.Core.WebServices.Interfaces
{
    public interface IWebServiceExtender
    {
        void Execute(TinyIoCContainer container, IPipelines pipelines);
    }
}