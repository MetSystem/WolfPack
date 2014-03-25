using System.IO;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Widgets;
using Wolfpack.Periscope.Core.Extensions;

namespace Wolfpack.Periscope.Core.Widgets.Raphy
{
    public class RaphyProgressWidget : WidgetBase, IWidget
    {
        public void RenderMarkup(TextWriter writer, WidgetConfiguration cfg)
        {
            //writer.WriteLine("<div id='" + cfg.Name + "' style='height:" + cfg.Height + "px;width:" + cfg.Width + "px;display:inline-block;'></div>");
        }

        public void RenderScript(TextWriter writer, WidgetConfiguration cfg)
        {
            // could inject different renders here...
            writer.WriteLine("<script>function " + cfg.Name + "(value) {jQuery('#" + cfg.Name + "').empty();var progress1 = new Charts.CircleProgress( '" + cfg.Name + "', '" + cfg.Title + "', value, { font_color: \"#fff\", fill_color: \"#222\", label_color: \"#333\", text_shadow: \"rgba(0,0,0,.4)\", stroke_color: \"#6a329e\" }); progress1.draw();}</script>");
            writer.WriteLine("<script>function " + cfg.Name + "Update(value){ console.log('updating " + cfg.Name + "!');" + cfg.Name + "(parseInt(value));}</script>");
            writer.WriteLine("<script>"+cfg.Name + "Update(0);</script>");
        }

        public WidgetDefinition Definition
        {
            get { return _definition; }            
        }

        private static readonly WidgetDefinition _definition;
        static RaphyProgressWidget()
        {
            _definition = new WidgetDefinition
                              {
                                  ImplementationType = typeof(RaphyProgressWidget).BuildTypeName()
                              };
        }

        public RaphyProgressWidget()
        {
            RegisterInclude("raphael", "/widgets/raphy/scripts/raphael-min.js");
            RegisterInclude("raphy", "/widgets/raphy/scripts/raphy-charts.js");
        }
    }
}