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

        private List<IWidget> _widgets;
        public IEnumerable<IWidget> Widgets { get { return _widgets; } set { _widgets = new List<IWidget>(value); } }

        public DashboardPanelConfiguration Add(IWidget widget)
        {
            if (_widgets == null)
                _widgets = new List<IWidget>();
            _widgets.Add(widget);
            return this;
        }
    }
}