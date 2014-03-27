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

        public PanelBuilder Add<TWidget>(Action<WidgetConfiguration> configurer, Func<WidgetConfiguration, IWidgetBootstrapper> builder = null)
            where TWidget : class, IWidget<WidgetConfiguration>, new()
        {
            return Add<TWidget, WidgetConfiguration>(configurer, builder);
        }

        public PanelBuilder Add<TWidget, TWidgetConfig>(Action<TWidgetConfig> configurer, Func<WidgetConfiguration, IWidgetBootstrapper> builder = null)
            where TWidgetConfig : WidgetConfiguration, new() 
            where TWidget : class, IWidget<TWidgetConfig>, new() 
        {
            var widget = new TWidget();
            _panel.Add(widget.Configure(configurer, builder).CreateInstance());
            return this;
        }

        public DashboardPanelConfiguration Build()
        {
            return _panel;
        }
    }
}