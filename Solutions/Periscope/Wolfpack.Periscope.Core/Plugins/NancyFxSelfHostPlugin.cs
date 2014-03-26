using System;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Hosting.Self;
using Nancy.TinyIoc;
using Wolfpack.Periscope.Core.Interfaces;

namespace Wolfpack.Periscope.Core.Plugins
{
    public class NancyFxSelfHostPlugin : IDashboardPlugin
    {
        private readonly IDashboard _dashboard;
        private readonly string _baseUrl;
        private NancyHost _host;

        public NancyFxSelfHostPlugin(IDashboard dashboard)
        {
            _dashboard = dashboard;

            // TODO: move this to some external config
            _baseUrl = "http://localhost:804";
        }

        public void Initialise()
        {
            _host = new NancyHost(new Uri(_baseUrl), new NancyFxDashboardBootstrapper(_dashboard));
        }

        public void Start()
        {
            _host.Start();
        }

        public void Dispose()
        {
            if (_host != null)
            {
                _host.Stop();
                _host.Dispose();
            }
        }
        private class NancyFxDashboardBootstrapper : DefaultNancyBootstrapper
        {
            private readonly IDashboard _dashboard;

            public NancyFxDashboardBootstrapper(IDashboard dashboard)
            {
                _dashboard = dashboard;
            }

            protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
            {
                container.Register(_dashboard);
            }

            protected override void ConfigureConventions(NancyConventions conventions)
            {
                base.ConfigureConventions(conventions);

                conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Scripts"));
                conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Widgets"));
            }
        }
    }
}