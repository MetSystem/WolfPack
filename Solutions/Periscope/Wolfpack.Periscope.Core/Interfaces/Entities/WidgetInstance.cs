using System.Collections.Generic;

namespace Wolfpack.Periscope.Core.Interfaces.Entities
{
    public class WidgetInstance
    {
        public List<Property> Includes { get; set; }
        public string Markup { get; set; }
        public string Script { get; set; }
        public WidgetDefinition Definition { get; set; }
        public WidgetConfiguration Configuration { get; set; }
        public IWidgetBootstrapper Bootstrapper{ get; set; }
    }
}