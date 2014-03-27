using System.IO;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Omu.ValueInjecter;
using Wolfpack.Periscope.Core.Extensions;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Events;
using Wolfpack.Periscope.Core.Interfaces.Messages;

namespace Wolfpack.Periscope.Core.Modules
{
    public class DashboardModule : NancyModule
    {
        private readonly IDashboard _dashboard;
        
        public DashboardModule(IDashboard dashboard)
            : base("/dashboard")
        {
            _dashboard = dashboard;

            Post["/update"] = _ =>
            {
                var update = this.Bind<WidgetUpdate>();
                _dashboard.Infrastructure.MessageBus.PublishAsync(new WidgetUpdateEvent(this, update));
                return new Response{ StatusCode = HttpStatusCode.Accepted };
            };

            Get["/about"] = _ => View["dashboard_about.sshtml"];
            Get["/"] = _ => View["dashboard_default.sshtml"];

            Get["/panel"] = _ =>
            {
                var panel = _dashboard.CurrentPanel;

                var response = new PanelResponse();
                response.InjectFrom<LoopValueInjection>(panel);

                response.Includes = panel.Widgets.SelectMany(w => w.Includes)
                    .Concat(_dashboard.Infrastructure.Includes)
                    .Distinct((a, b) => a.Name.Equals(b.Name))
                    .ToList();

                response.Widgets = panel.Widgets.ToList();

                return response;
            };
        }
    }
}