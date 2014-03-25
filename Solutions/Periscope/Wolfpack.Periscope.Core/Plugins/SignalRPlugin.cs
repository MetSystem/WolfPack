using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using Wolfpack.Periscope.Core.Infrastructure;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Events;

namespace Wolfpack.Periscope.Core.Plugins
{
    public class SignalRPlugin : IDashboardPlugin
    {
        private IDisposable _appHost;

        private readonly List<TinyMessageSubscriptionToken> _subscriptionTokens;
        private readonly IDashboard _dashboard;

        public SignalRPlugin(IDashboard dashboard)
        {
            _dashboard = dashboard;
            _subscriptionTokens = new List<TinyMessageSubscriptionToken>();

            _dashboard.Infrastructure.Includes.Add(new Property
                                             {
                                                 Name = "signalr-client",
                                                 Value = "/Scripts/jquery.signalR-2.0.2.min.js"
                                             });
            _dashboard.Infrastructure.Includes.Add(new Property
                                             {
                                                 Name = "signalr-hubs",
                                                 // TODO: pass this as ctor config param
                                                 Value = "http://localhost:805/signalr/hubs"
                                             });
            _dashboard.Infrastructure.Includes.Add(new Property
                                             {
                                                 Name = "signalr-periscope",
                                                 Value = "/Scripts/periscope-signalr.js"
                                             });
        }

        public void Initialise()
        {
            _subscriptionTokens.Add(_dashboard.Infrastructure.MessageBus.Subscribe<ClockTickEvent>(BroadcastClockTick));
            _subscriptionTokens.Add(_dashboard.Infrastructure.MessageBus.Subscribe<WidgetUpdateEvent>(BroadcastWidgetUpdate));
            _subscriptionTokens.Add(_dashboard.Infrastructure.MessageBus.Subscribe<PanelChangedEvent>(BroadcastPanelChanged));
        }

        public void Start()
        {
            // TODO: pass this in as ctor param
            _appHost = WebApp.Start<Startup>("http://localhost:805/");
        }

        private void BroadcastPanelChanged(PanelChangedEvent payload)
        {
            GlobalHost.ConnectionManager.GetHubContext<Dashboard>().Clients.All.panelChanged(
                new { payload.Content.Id, payload.Content.Name, payload.Content.DwellInSeconds });
        }

        private void BroadcastClockTick(ClockTickEvent payload)
        {
            GlobalHost.ConnectionManager.GetHubContext<Dashboard>().Clients.All.clockTick(payload.Content);
        }

        private void BroadcastWidgetUpdate(WidgetUpdateEvent payload)
        {
            GlobalHost.ConnectionManager.GetHubContext<Dashboard>().Clients.All.widgetUpdate(payload.Content);
        }

        public void Dispose()
        {            
            _subscriptionTokens.ForEach(t => t.Dispose());
            _appHost.Dispose();
        }
    }

    public class Dashboard : Hub
    {
        public override System.Threading.Tasks.Task OnConnected()
        {
            Debug.WriteLine("Connected!");
            return base.OnConnected();
        }
    }

    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                // Setup the cors middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);

                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    // EnableJSONP = true
                };

                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch is already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}