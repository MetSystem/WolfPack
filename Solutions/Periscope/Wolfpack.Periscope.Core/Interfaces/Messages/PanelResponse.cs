using System;
using System.Collections.Generic;
using Wolfpack.Periscope.Core.Interfaces.Entities;

namespace Wolfpack.Periscope.Core.Interfaces.Messages
{
    public class PanelResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public int DwellInSeconds { get; set; }

        private List<WidgetInstance> _widgets;
        public IEnumerable<WidgetInstance> Widgets { get { return _widgets; } set { _widgets = new List<WidgetInstance>(value); } }

        public List<Property> Includes { get; set; }

        public PanelResponse()
        {
            Includes = new List<Property>();
        }
    }
}