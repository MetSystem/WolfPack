using System.IO;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Widgets;
using Wolfpack.Periscope.Core.Extensions;

namespace Wolfpack.Periscope.Core.Providers.Highcharts
{
    public class HighchartBarChart : WidgetBase, IWidget
    {
        public void RenderMarkup(TextWriter writer, WidgetConfiguration cfg)
        {
            throw new System.NotImplementedException();
        }

        public void RenderScript(TextWriter writer, WidgetConfiguration cfg)
        {
            // could inject different renders here...
            writer.WriteLine("<chart type='bar'></chart>");
        }

        public WidgetDefinition Definition
        {
            get { return _definition; }            
        }

        private static readonly WidgetDefinition _definition;
        static HighchartBarChart()
        {
            _definition = new WidgetDefinition
                              {
                                  ImplementationType = typeof(HighchartBarChart).BuildTypeName()
                              };
        }

        public HighchartBarChart()
        {
            RegisterInclude("highcharts", "/widgets/highcharts/scripts/highcharts.js");
        }
    }
}