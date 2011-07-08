namespace Wolfpack.Core.Geckoboard.Entities
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Interfaces.Entities;

    [DataContract(Name = "points", Namespace = "")]
    public class GeckoMap
    {
        [DataMember(Name = "point")]
        public List<GeoData> Points { get; set; }
    }
}