using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces;

namespace Wolfpack.Core.WebUI
{
    public class WebServiceUIStartup : IStartupPlugin
    {
        public Status Status { get; set; }
        public void Initialise()
        {
            Container.RegisterAsSingleton<IWebServiceExtender>(typeof (WebUIServiceExtender));
        }
    }
}