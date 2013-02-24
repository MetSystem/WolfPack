using System.Collections.Generic;

namespace Wolfpack.Contrib.Geckoboard.Entities
{
    public class GeckoLineChart : GeckoData
    {
        public class Setting : GeckoData
        {
            public List<string> X { get; set; }
            public List<string> Y { get; set; }
            public string Colour { get; set; }

            public Setting()
            {
                X = new List<string>();
                Y = new List<string>();
            }
        }

        public List<string> Item { get; set; }

        public Setting Settings { get; set; }

        public GeckoLineChart()
        {
            Item = new List<string>();
            Settings = new Setting();
        }
    }
}