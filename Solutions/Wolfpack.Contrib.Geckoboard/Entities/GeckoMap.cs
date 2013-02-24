using System.Collections.Generic;

namespace Wolfpack.Contrib.Geckoboard.Entities
{
    public class GeckoMap
    {
        public MapDataPoints Points { get; set; }
    }

    public class MapDataPoints
    {
        public List<MapData> Points { get; set; }
    }

    public class MapData
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}