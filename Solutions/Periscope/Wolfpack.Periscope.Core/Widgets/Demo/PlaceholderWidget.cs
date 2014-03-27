using System.IO;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Widgets;

namespace Wolfpack.Periscope.Core.Widgets.Demo
{
    public class PlaceholderWidget : WidgetBase<WidgetConfiguration>
    {
        public override void RenderMarkup(TextWriter writer)
        {
            writer.WriteLine("<div style='height:100%;width:100%;background-color: orange;float:left;'>Placeholder: {0}</div>", Configuration.Name);
        }

        public override void RenderScript(TextWriter writer)
        {            
        }
    }
}