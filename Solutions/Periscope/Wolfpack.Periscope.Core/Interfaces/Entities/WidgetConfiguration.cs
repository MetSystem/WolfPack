using System.Collections.Generic;

namespace Wolfpack.Periscope.Core.Interfaces.Entities
{
    public class WidgetConfiguration
    {
        public string Name { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string Title { get; set; }
        public List<Property> CustomProperties { get; set; }        

        public WidgetConfiguration()
        {
            Row = 1;
            Column = 1;
            Height = 1;
            Width = 1;
        }
    }
}