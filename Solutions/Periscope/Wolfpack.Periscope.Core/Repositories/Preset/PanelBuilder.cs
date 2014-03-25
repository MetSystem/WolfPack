using System;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;

namespace Wolfpack.Periscope.Core.Repositories.Preset
{
    public class PanelBuilder
    {
        private readonly DashboardPanelConfiguration _panel;

        public PanelBuilder(string name)
        {
            _panel = new DashboardPanelConfiguration
                         {
                             Name = name
                         };
        }

        public PanelBuilder SetDwellTime(int intervalInSeconds)
        {
            _panel.DwellInSeconds = intervalInSeconds;
            return this;
        }

        public PanelBuilder Add<T>(Action<WidgetConfiguration> configurer, Func<WidgetConfiguration, IWidgetBootstrapper> builder = null)
            where T: class, IWidget, new() 
        {
            var config = new WidgetConfiguration();
            configurer(config);

            var widget = new T { Configuration = config };
            _panel.Add(widget);

            if (builder != null)
            {
                var bootstrapper = builder(config);
                if (bootstrapper != null)
                    widget.Bootstrapper = bootstrapper;
            }

            return this;
        }

        public DashboardPanelConfiguration Build()
        {
            return _panel;
        }
    }
}