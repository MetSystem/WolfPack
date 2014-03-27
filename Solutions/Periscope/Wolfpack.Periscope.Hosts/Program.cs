using System;
using Topshelf;
using Wolfpack.Periscope.Core;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Events;
using Wolfpack.Periscope.Core.Plugins;
using Wolfpack.Periscope.Core.Repositories.Preset;
using Wolfpack.Periscope.Core.Widgets;
using Wolfpack.Periscope.Core.Widgets.Pumps;

namespace Wolfpack.Periscope.Hosts
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(
                config =>
                {
                    config.SetDisplayName("Wolfpack Periscope");
                    config.SetServiceName("Periscope");
                    config.SetDescription("Wolfpack Periscope Dashboard Service");

                    string username;
                    string password;

                    //if (CmdLine.Value(CmdLine.SwitchNames.Username, out username) &&
                    //    CmdLine.Value(CmdLine.SwitchNames.Password, out password))
                    //{
                    //    Logger.Debug("Running As: {0}", username);
                    //    config.RunAs(username, password);
                    //}

                    config.Service<IDashboard>(configurator => 
                    {
                        configurator.ConstructUsing(factory =>
                        {
                            var bootstrapper = new DashboardBootstrapper();
                            var dashboard = bootstrapper.Configure(infrastructure =>
                            {
                                var panelBuilder = DashboardConfigurationBuilder.New()
                                    .Add("PanelA", (infra, builder) => builder.Add<TemplatedWidget<TemplatedWidgetConfiguration>, TemplatedWidgetConfiguration>(
                                        cfg =>
                                        {
                                            cfg.Title = "ClockWidget";
                                            cfg.Name = "Time";
                                            cfg.Height = 250;
                                            cfg.Width = 550;
                                            cfg.MarkupTemplate = "clock";
                                            cfg.ScriptTemplate = "clock";
                                            cfg.RegisterInclude("moment", "/scripts/moment-min.js")
                                                .AddTemplateData(new {FontSize = "20px"});
                                        },
                                        cfg => new DataPumpBootstrapper<IntervalDataPump>(infra,
                                            new IntervalDataPump(infra, TimeSpan.FromSeconds(1)), cfg,
                                            () =>
                                            {
                                                var payload = DateTime.Now.ToString("s");
                                                infra.MessageBus.Publish(new WidgetUpdateEvent(infra, new WidgetUpdate
                                                {
                                                    Payload = payload,
                                                    Target = cfg.Name
                                                }));

                                                infra.Logger.LogDebug("Callback fired {0} on widget '{1}'", payload, cfg.Name);
                                            }))
                                        .SetDwellTime(60));


                                infrastructure.Container.RegisterInstance(panelBuilder);
                                infrastructure.Container.RegisterType<IDashboardConfigurationRepository, PresetRepository>();


                                infrastructure.RegisterPlugin<SignalRPlugin>();
                                infrastructure.RegisterPlugin<NancyFxSelfHostPlugin>();
                                infrastructure.RegisterPlugin<RotatingPanelSelectorPlugin>();
                                infrastructure.RegisterPlugin<BootstrapWidgetsPlugin>();
                            });

                            return dashboard;
                        });

                        configurator.WhenStarted(s => s.Start());
                        configurator.WhenStopped(s => s.Dispose());
                    });
                });
        }
    }
}
