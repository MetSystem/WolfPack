using System.IO;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Widgets;

namespace Wolfpack.Periscope.Core.Widgets.Raphy
{
    public class RaphyProgressWidget : WidgetBase<WidgetConfiguration>
    {
        public override void RenderMarkup(TextWriter writer)
        {
            //writer.WriteLine("<div id='" + cfg.Name + "' style='height:" + cfg.Height + "px;width:" + cfg.Width + "px;display:inline-block;'></div>");
        }

        public override void RenderScript(TextWriter writer)
        {
            // could inject different renders here...
            writer.WriteLine("<script>function " + Configuration.Name + "(value) {jQuery('#" + Configuration.Name + "').empty();var progress1 = new Charts.CircleProgress( '" + Configuration.Name + "', '" + Configuration.Title + "', value, { font_color: \"#fff\", fill_color: \"#222\", label_color: \"#333\", text_shadow: \"rgba(0,0,0,.4)\", stroke_color: \"#6a329e\" }); progress1.draw();}</script>");
            writer.WriteLine("<script>function " + Configuration.Name + "Update(value){ console.log('updating " + Configuration.Name + "!');" + Configuration.Name + "(parseInt(value));}</script>");
            writer.WriteLine("<script>" + Configuration.Name + "Update(0);</script>");
        }

        public RaphyProgressWidget()
        {
            RegisterInclude("raphael", "/widgets/raphy/scripts/raphael-min.js");
            RegisterInclude("raphy", "/widgets/raphy/scripts/raphy-charts.js");
        }
    }
}