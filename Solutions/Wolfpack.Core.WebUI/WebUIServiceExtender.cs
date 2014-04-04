using System.Collections.Generic;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Wolfpack.Core.WebServices.Interfaces;
using Wolfpack.Core.WebUI.Modules;

namespace Wolfpack.Core.WebUI
{
    public class WebUIServiceExtender : IWebServiceExtender
    {
        private readonly IEnumerable<INeedMenuSpace> _menuChangers;

        public WebUIServiceExtender(IEnumerable<INeedMenuSpace> menuChangers)
        {
            _menuChangers = menuChangers;
        }
        public IEnumerable<ModuleRegistration> Modules
        {
            get
            {
                return new[]
                {
                    new ModuleRegistration(typeof(UiApiModule)), 
                    new ModuleRegistration(typeof(UiViewsModule)), 
                };
            }
        }

        public void Execute(TinyIoCContainer container, IPipelines pipelines)
        {
            var menuTwiddler = new MenuChanger(_menuChangers);
            container.Register(menuTwiddler);
        }
    }
}