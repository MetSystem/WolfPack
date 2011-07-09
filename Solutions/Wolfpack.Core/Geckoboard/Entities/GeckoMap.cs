namespace Wolfpack.Core.Geckoboard.Entities
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract(Name = "root", Namespace = "")]
    public class GeckoMap
    {
        [DataMember(Name = "points")]
        public MapDataPoints Points { get; set; }
    }

    [DataContract(Namespace = "")]
    public class MapDataPoints
    {
        [DataMember(Name = "point")]
        public List<MapData> Points { get; set; }
    }

    [DataContract(Namespace = "")]
    public class MapData
    {
        [DataMember(Name = "longitude")]
        public string Longitude { get; set; }
        [DataMember(Name = "latitude")]
        public string Latitude { get; set; }
    }
}