using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Threading;
using NUnit.Framework;
using Wolfpack.Periscope.Core;
using Wolfpack.Periscope.Core.Infrastructure;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Events;
using Wolfpack.Periscope.Core.Interfaces.Messages;
using Wolfpack.Periscope.Core.Plugins;
using Wolfpack.Periscope.Core.Repositories.Preset;
using Wolfpack.Periscope.Core.Widgets;
using Wolfpack.Periscope.Core.Widgets.Demo;
using Wolfpack.Periscope.Core.Widgets.Highcharts;
using Wolfpack.Periscope.Core.Widgets.Pumps;
using Wolfpack.Periscope.Core.Widgets.Raphy;
using Wolfpack.Periscope.Tests.Bdd;
using System.Linq;

namespace Wolfpack.Periscope.Tests
{
    public class SystemDomain : BddTestDomain
    {
        private PanelResponse _currentPanel;
        private IDashboard _dashboard;

        public const string BaseUrl = "http://localhost:8082/";
        private DashboardConfigurationBuilder _panelBuilder;


        private TinyMessageSubscriptionToken _panelChangeSubscriptionToken;
        private readonly List<PanelChangeRequestEvent> _panelChangeEvents;

        private TinyMessageSubscriptionToken _clockSubscriptionToken;
        private readonly List<ClockTickEvent> _clockTickEvents;

        public SystemDomain()
        {
            _clockTickEvents = new List<ClockTickEvent>();
            _panelChangeEvents = new List<PanelChangeRequestEvent>();

            Debug.WriteLine("Created TickCount {0}", _clockTickEvents.GetHashCode());
        }

        public override void Dispose()
        {
            _clockSubscriptionToken.Dispose();
            _panelChangeSubscriptionToken.Dispose();
        }

        public void TheDashboardIsStarted()
        {
            var bootstrapper = new DashboardBootstrapper();
            _dashboard = bootstrapper.Configure(infrastructure =>
                               {
                                   infrastructure.Container.RegisterInstance(_panelBuilder);
                                   infrastructure.Container.RegisterType<IDashboardConfigurationRepository, PresetRepository>();


                                   infrastructure.RegisterPlugin<SignalRPlugin>();
                                   infrastructure.RegisterPlugin<NancyFxSelfHostPlugin>();
                                   infrastructure.RegisterPlugin<RotatingPanelSelectorPlugin>();
                                   infrastructure.RegisterPlugin<BootstrapWidgetsPlugin>();

                                   // snoop (and assert) on the internal bus events...
                                   _clockSubscriptionToken = infrastructure.MessageBus.Subscribe<ClockTickEvent>(RecordClockTickEvent);
                                   _panelChangeSubscriptionToken = infrastructure.MessageBus.Subscribe<PanelChangeRequestEvent>(RecordPanelChangeEvent);
                               });            

            _dashboard.Start();
        }

        private void RecordPanelChangeEvent(PanelChangeRequestEvent changeEvent)
        {
            _panelChangeEvents.Add(changeEvent);
            Debug.WriteLine("[PanelChangeRequestEvent] {0} '{1}', Dwell:={2}s, Seq:={3}", changeEvent.Content.Id,
                changeEvent.Content.Name,
                changeEvent.Content.DwellInSeconds,
                changeEvent.Content.Sequence);
        }

        private void RecordClockTickEvent(ClockTickEvent tickEvent)
        {
            _clockTickEvents.Add(tickEvent);
            Debug.WriteLine("[ClockTickEvent] {0}, Tick[{1}]: {2}", _clockTickEvents.GetHashCode(), _clockTickEvents.Count, tickEvent.Content);
        }

        public void WeCreateAPanelNamed_WithSomeDefaultWidgets(string name)
        {
            _panelBuilder = DashboardConfigurationBuilder.New().Add("PanelA", 
                (infra, builder) => builder

                    .Add<RaphyProgressWidget>(
                    cfg =>
                    {
                        cfg.Title = "Disk Space Used";
                        cfg.Name = "widget1";
                        cfg.Height = 250;
                        cfg.Width = 250;
                    },

                    cfg => new DataPumpBootstrapper<IntervalDataPump>(infra,
                        new IntervalDataPump(infra, TimeSpan.FromSeconds(7)), cfg,
                        () =>
                        {
                            // this callback allows us to inject the custom data look up
                            // into the widget update - simply publish a WidgetUpdateEvent 
                            // with the data the widget is expecting to receive and SignalR
                            // will deliver it to the dashboard.

                            // get next data value====================>
                            // could be a webservice lookup, database load...anything...
                            var rnd = new Random();
                            var value = rnd.Next(0, 100);
                            infra.MessageBus.Publish(new WidgetUpdateEvent(this, new WidgetUpdate
                            {
                                Payload = value.ToString(CultureInfo.InvariantCulture),
                                Target = cfg.Name
                            }));

                            infra.Logger.LogDebug("Callback fired value {0} at {1}!", value, cfg.Name);
                        }))

                .Add<RaphyProgressWidget>(
                    cfg =>
                    {
                        cfg.Title = "CPU";
                        cfg.Name = "widget2";
                        cfg.Height = 250;
                        cfg.Width = 250;
                    },

                    cfg => new DataPumpBootstrapper<IntervalDataPump>(infra,
                        new IntervalDataPump(infra, TimeSpan.FromSeconds(17)), cfg,
                        () =>
                        {
                            var rnd = new Random();
                            var value = rnd.Next(0, 100);
                            infra.MessageBus.Publish(new WidgetUpdateEvent(this, new WidgetUpdate
                            {
                                Payload = value.ToString(CultureInfo.InvariantCulture),
                                Target = cfg.Name
                            }));

                            infra.Logger.LogDebug("Callback fired value {0} at {1}!", value, cfg.Name);
                        }))
                
                .Add<PlaceholderWidget>(
                    cfg =>
                        {
                            cfg.Title = "PlaceHolder4";
                            cfg.Name = "widget4";
                            cfg.Height = 250;
                            cfg.Width = 250;
                        })

                .Add<HighchartPieChart>(
                    cfg =>
                        {
                            cfg.Title = "Hightcharts!";
                            cfg.Name = "widget99";
                            cfg.Height = 250;
                            cfg.Width = 450;
                        })
                // panel displays for...
                .SetDwellTime(60)
           );
        }

        public void WeWait_Seconds(int intervalMilliseconds)
        {
            Thread.Sleep(intervalMilliseconds);
        }

        public void ThereShouldBe_ClockTickEventsGenerated(int expected)
        {
            Debug.WriteLine("{0} actual tick events", _clockTickEvents.Count);
            Assert.That(_clockTickEvents.Count, Is.GreaterThanOrEqualTo(expected));
        }

        public void ThePanelRequestApiCallIsMade()
        {
            var url = BaseUrl + "dashboard";

            if (_currentPanel != null)
                url += string.Format("?current={0}", _currentPanel.Id);

            var client = new WebClient();
            client.Headers.Add(HttpRequestHeader.Accept, "application/json");
            var responseJson = client.DownloadString(url);
           // _currentPanel = 
        }

        public void ThePanelShouldContain_Widgets(int expected)
        {
            Assert.That(_currentPanel.Widgets.Count(), Is.EqualTo(expected));
        }
    }
}