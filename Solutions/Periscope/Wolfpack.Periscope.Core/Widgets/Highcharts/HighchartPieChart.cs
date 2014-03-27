using System.IO;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Widgets;

namespace Wolfpack.Periscope.Core.Widgets.Highcharts
{
    public class HighchartPieChart : WidgetBase<WidgetConfiguration>
    {
        public override void RenderMarkup(TextWriter writer)
        {            
        }

        public override void RenderScript(TextWriter writer)
        {
            // could inject different renders here...
            writer.WriteLine("<script>function " + Configuration.Name + "() {jQuery('#" + Configuration.Name + "').highcharts({ chart: { plotBackgroundColor: null, plotBorderWidth: null, plotShadow: false }, title: { text: '', floating: true }, tooltip: { pointFormat: '{series.name}: <b>{point.percentage}%</b>', percentageDecimals: 1 }, plotOptions: { pie: { allowPointSelect: true, cursor: 'pointer', dataLabels: { enabled: true, color: '#000000', connectorColor: '#000000', formatter: function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' %'; } } } }, series: [{ type: 'pie', name: 'Browser share', data: [ ['Firefox', 45.0], ['IE', 26.8], { name: 'Chrome', y: 12.8, sliced: true, selected: true }, ['Safari', 8.5], ['Opera', 6.2], ['Others', 0.7] ] }] });  }</script>");
            writer.WriteLine("<script>function " + Configuration.Name + "Update(value){ console.log('updating " + Configuration.Name + "! '+value);}</script>");
            writer.WriteLine("<script>" + Configuration.Name + "();</script>");
        }

        public HighchartPieChart()
        {
            RegisterInclude("highcharts", "/widgets/highcharts/scripts/highcharts.js");
            RegisterInclude("highcharts-theme-periscope", "/widgets/highcharts/scripts/themes/periscope.js");
        }
    }
}