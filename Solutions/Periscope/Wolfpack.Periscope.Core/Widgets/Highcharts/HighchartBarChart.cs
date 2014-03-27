using System.IO;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Widgets;

namespace Wolfpack.Periscope.Core.Widgets.Highcharts
{
    public class HighchartBarChart : WidgetBase<WidgetConfiguration>
    {
        public override void RenderMarkup(TextWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override void RenderScript(TextWriter writer)
        {
            // could inject different renders here...
            writer.WriteLine("<chart type='bar'></chart>");
        }

        public HighchartBarChart()
        {
            RegisterInclude("highcharts", "/widgets/highcharts/scripts/highcharts.js");
        }
    }
}