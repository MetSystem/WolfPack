using System.IO;
using Wolfpack.Periscope.Core.Extensions;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Widgets;

namespace Wolfpack.Periscope.Core.Providers.Demo
{
    public class PlaceholderWidget : WidgetBase, IWidget
    {
        public void RenderMarkup(TextWriter writer, WidgetConfiguration cfg)
        {
            writer.WriteLine("<div style='height:100%;width:100%;background-color: orange;float:left;'>Placeholder: {0}</div>", cfg.Name);
        }

        public void RenderScript(TextWriter writer, WidgetConfiguration cfg)
        {            
        }

        public WidgetDefinition Definition
        {
            get { return _definition; }            
        }

        private static readonly WidgetDefinition _definition;
        static PlaceholderWidget()
        {
            _definition = new WidgetDefinition
                              {
                                  ImplementationType = typeof(PlaceholderWidget).BuildTypeName()
                              };
        }
    }
}