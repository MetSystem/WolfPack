using System;

namespace Wolfpack.Contrib.Geckoboard.Entities
{
    public class GeckoDataItem : GeckoData
    {        
        private double myValue;

        public double Value
        {
            get { return Math.Round(myValue, DecimalPlaces); }
            set { myValue = value; }
        }
        public string Text { get; set; }
    }
}