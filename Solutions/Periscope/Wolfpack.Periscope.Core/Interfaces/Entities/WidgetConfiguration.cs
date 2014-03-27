using System.Collections.Generic;

namespace Wolfpack.Periscope.Core.Interfaces.Entities
{
    public class WidgetConfiguration
    {
        public string Name { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Title { get; set; }

        public WidgetConfiguration()
        {
            Height = 250;
            Width = 250;
        }
    }
}