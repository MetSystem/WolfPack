using Nancy;

namespace Wolfpack.Core.WebUI.Modules
{
    public class UiViewsModule : NancyModule
    {
        private const string BaseUrl = "/ui";

        public UiViewsModule() 
            : base(BaseUrl)
        {
            Get["/"] = _ => View["views/status.sshtml"];
            Get["/status"] = _ => View["views/status.sshtml"];
            Get["/activity"] = _ => View["views/activity.sshtml"];
            Get["/gallery"] = _ => View["views/gallery.sshtml"];
            Get["/configure"] = _ => View["views/configure.sshtml"];
            Get["/about"] = _ => View["views/about.sshtml"];
        }
    }
}