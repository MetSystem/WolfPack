using System;
using System.Collections.Generic;

namespace Wolfpack.Contrib.Geckoboard.Entities
{
    public class GeckoPieChart
    {
        public class Piece : GeckoData
        {
            private double myValue;

            public double Value
            {
                get { return Math.Round(myValue, DecimalPlaces); }
                set { myValue = value; }
            }
            public string Label { get; set; }
            public string Colour { get; set; }
        }

        public List<Piece> Item { get; set; }

        public GeckoPieChart()
        {
            Item = new List<Piece>();
        }
    }
}