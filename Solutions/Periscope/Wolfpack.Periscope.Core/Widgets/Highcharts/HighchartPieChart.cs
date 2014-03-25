using System.Collections.Generic;
using System.IO;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Widgets;
using Wolfpack.Periscope.Core.Extensions;

namespace Wolfpack.Periscope.Core.Providers.Highcharts
{
    public class HighchartPieChart : WidgetBase, IWidget
    {
        public void RenderMarkup(TextWriter writer, WidgetConfiguration cfg)
        {            
        }

        public void RenderScript(TextWriter writer, WidgetConfiguration cfg)
        {
            // could inject different renders here...
            writer.WriteLine("<script>function " + cfg.Name + "() {jQuery('#" + cfg.Name + "').highcharts({ chart: { plotBackgroundColor: null, plotBorderWidth: null, plotShadow: false }, title: { text: '', floating: true }, tooltip: { pointFormat: '{series.name}: <b>{point.percentage}%</b>', percentageDecimals: 1 }, plotOptions: { pie: { allowPointSelect: true, cursor: 'pointer', dataLabels: { enabled: true, color: '#000000', connectorColor: '#000000', formatter: function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' %'; } } } }, series: [{ type: 'pie', name: 'Browser share', data: [ ['Firefox', 45.0], ['IE', 26.8], { name: 'Chrome', y: 12.8, sliced: true, selected: true }, ['Safari', 8.5], ['Opera', 6.2], ['Others', 0.7] ] }] });  }</script>");
            writer.WriteLine("<script>function " + cfg.Name + "Update(value){ console.log('updating " + cfg.Name + "! '+value);}</script>");
            writer.WriteLine("<script>" + cfg.Name + "();</script>");
        }

        public WidgetDefinition Definition
        {
            get { return _definition; }            
        }

        private static readonly WidgetDefinition _definition;
        static HighchartPieChart()
        {
            _definition = new WidgetDefinition
                              {
                                  ImplementationType = typeof(HighchartPieChart).BuildTypeName()
                              };
        }

        public HighchartPieChart()
        {
            RegisterInclude("highcharts", "/widgets/highcharts/scripts/highcharts.js");
            RegisterInclude("highcharts-theme-periscope", "/widgets/highcharts/scripts/themes/periscope.js");
        }
    }
}