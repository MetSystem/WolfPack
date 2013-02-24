namespace Wolfpack.Contrib.Geckoboard.Entities
{
    public class GeckoComparison : GeckoData
    {
        public GeckoDataItem[] Item { get; set; }

        public GeckoComparison()
        {
            Item = new[]
                       {
                           new GeckoDataItem(),
                           new GeckoDataItem()
                       };
        }

        public GeckoComparison(int decimalPlaces)
            : this()
        {
            Item[0].DecimalPlaces = decimalPlaces;
            Item[1].DecimalPlaces = decimalPlaces;
        }

        public GeckoDataItem Number 
        {
            get { return Item[0]; }
            set { Item[0] = value; }
        }

        public GeckoDataItem Comparison 
        {
            get { return Item[1]; }
            set { Item[1] = value; }
        }
    }
}