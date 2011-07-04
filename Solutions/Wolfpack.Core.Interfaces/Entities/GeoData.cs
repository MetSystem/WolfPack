namespace Wolfpack.Core.Interfaces.Entities
{
    public class GeoData
    {
        public CityGeoData City { get; set; }
        public PointGeoData Point { get; set; }
        public DnsGeoData Dns { get; set; }
    }

    public class CityGeoData
    {
        public string CountryCode { get; set; }
        public string RegionCode { get; set; }
        public string City { get; set; }
    }

    public class PointGeoData
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }

    public class DnsGeoData
    {
        public string Hostname { get; set; }
        public string IpAddress { get; set; }
    }
}