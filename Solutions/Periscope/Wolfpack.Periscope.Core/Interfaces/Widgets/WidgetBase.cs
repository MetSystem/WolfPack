
using System.Collections.Generic;
using Wolfpack.Periscope.Core.Interfaces.Entities;

namespace Wolfpack.Periscope.Core.Interfaces.Widgets
{
    public class WidgetBase
    {
        public IWidgetBootstrapper Bootstrapper { get; set; }
        public List<Property> Includes { get; set; }
        public WidgetConfiguration Configuration { get; set; }

        public WidgetBase()
        {
            Includes = new List<Property>();
        }

        public void RegisterInclude(string name, string link)
        {
            Includes.Add(new Property{ Name = name, Value = link });
        }
    }
}