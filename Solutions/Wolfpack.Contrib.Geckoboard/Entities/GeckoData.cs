using System;
using System.Collections.Generic;
using System.Linq;

namespace Wolfpack.Contrib.Geckoboard.Entities
{
    public class GeckoData
    {
        public virtual int DecimalPlaces { get; set; }
    }

    public class GeckoValues : List<double>
    {
        public double Mid()
        {
            return (this.Min() + ((this.Max() - this.Min())/2));
        }

        public double Mid(int dp)
        {
            return Math.Round(Mid(), dp);
        }
    }
}