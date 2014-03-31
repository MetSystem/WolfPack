using Nancy;
using Wolfpack.Core.WebServices.Interfaces.Messages;

namespace Wolfpack.Core.WebUI.Modules
{
    public class UiApiModule : NancyModule
    {
        private const string BaseUrl = "/api";

        private readonly MenuChanger _menuChanger;

        public UiApiModule(MenuChanger menuChanger) 
            : base(BaseUrl)
        {
            _menuChanger = menuChanger;

            Get["/addonmenu"] = GetAddOnMenu;
        }

        private dynamic GetAddOnMenu(dynamic request)
        {
            var menuChanges = _menuChanger.BuildChanges();
            return Response.AsJson(new MenuChangeResponse { Changes = menuChanges });
        }
    }
}