using System.Collections.Generic;

namespace Wolfpack.Periscope.Core.Interfaces.Widgets
{
    public class DataSeries
    {
        public string Name { get; set; }
        public List<double> Values { get; set; }
    }
}