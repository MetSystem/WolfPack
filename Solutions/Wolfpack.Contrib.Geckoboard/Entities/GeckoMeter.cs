using System;

namespace Wolfpack.Contrib.Geckoboard.Entities
{
    public class GeckoMeter : GeckoData
    {
        private double myItem;

        public double Item
        {
            get { return Math.Round(myItem, DecimalPlaces); }
            set { myItem = value; }
        }

        public GeckoDataItem Min { get; set; }

        public GeckoDataItem Max { get; set; }

        public GeckoMeter()
        {
            Min = new GeckoDataItem();
            Max = new GeckoDataItem();
        }

        public override int DecimalPlaces
        {
            get { return base.DecimalPlaces; }
            set
            {
                base.DecimalPlaces = value;

                if (Min != null)
                    Min.DecimalPlaces = value;
                if (Max != null)
                    Max.DecimalPlaces = value;
            }
        }
    }
}