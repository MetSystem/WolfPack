using System;
using System.Collections.Generic;

namespace Wolfpack.Periscope.Core.Interfaces.Entities
{
    public class DashboardPanelConfiguration : IDashboardPanel
    {
        public DashboardPanelConfiguration()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public int DwellInSeconds { get; set; }

        private List<WidgetInstance> _widgets;
        public IEnumerable<WidgetInstance> Widgets { get { return _widgets; } set { _widgets = new List<WidgetInstance>(value); } }

        public DashboardPanelConfiguration Add(WidgetInstance widget)
        {
            if (_widgets == null)
                _widgets = new List<WidgetInstance>();
            _widgets.Add(widget);
            return this;
        }
    }
}