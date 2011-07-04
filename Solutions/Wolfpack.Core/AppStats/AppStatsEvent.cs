using System;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.AppStats
{
    public class AppStatsEvent : AppStatsEventExtensions.IAppStatsPieChartContinuation<AppStatsEvent>,
        AppStatsEventExtensions.IAppStatsGeoContinuation<AppStatsEvent>
    {       
        public string SiteId { get; set; }
        public string AgentId { get; set; }
        public string CheckId { get; set; }
        public string Tag { get; set; }
        public TimeSpan? Duration { get; set; }
        public double? ResultCount { get; set; }
        public int? MinuteBucket { get; set; }
        public int? HourBucket { get; set; }
        public int? DayBucket { get; set; }
        public GeoData Geo { get; set; }

        public AppStatsEvent()
        {
            MinuteBucket = (int)DateTime.UtcNow.Subtract(HealthCheckResult.BucketBaselineDate).TotalMinutes;
            HourBucket = (int)DateTime.UtcNow.Subtract(HealthCheckResult.BucketBaselineDate).TotalHours;
            DayBucket = (int)DateTime.UtcNow.Subtract(HealthCheckResult.BucketBaselineDate).TotalDays;
        }

        public AppStatsEvent One()
        {
            return Count(1);            
        }

        public AppStatsEvent Count(double count)
        {
            ResultCount = count;
            return this;
        }

        public AppStatsEvent Time(TimeSpan duration)
        {
            Duration = duration;
            ResultCount = duration.TotalMilliseconds;
            return this;
        }

        AppStatsEvent AppStatsEventExtensions.IAppStatsPieChartContinuation<AppStatsEvent>.Segment(string id)
        {
            Tag = id;
            return this;
        }

        AppStatsEvent AppStatsEventExtensions.IAppStatsGeoContinuation<AppStatsEvent>.IpAddress(string ipAddress)
        {
            if (Geo == null)
                Geo = new GeoData();
            if (Geo.Dns == null)
                Geo.Dns = new DnsGeoData();
            Geo.Dns.IpAddress = ipAddress;
            return this;
        }

        AppStatsEvent AppStatsEventExtensions.IAppStatsGeoContinuation<AppStatsEvent>.Hostname(string hostName)
        {
            if (Geo == null)
                Geo = new GeoData();
            if (Geo.Dns == null)
                Geo.Dns = new DnsGeoData();
            Geo.Dns.Hostname = hostName;
            return this;
        }

        AppStatsEvent AppStatsEventExtensions.IAppStatsGeoContinuation<AppStatsEvent>.City(string countryCode, string city)
        {
            if (Geo == null)
                Geo = new GeoData();
            if (Geo.City == null)
                Geo.City = new CityGeoData();
            Geo.City.CountryCode = countryCode;
            Geo.City.City = city;
            return this;
        }

        AppStatsEvent AppStatsEventExtensions.IAppStatsGeoContinuation<AppStatsEvent>.City(string countryCode, string regionCode, string city)
        {
            if (Geo == null)
                Geo = new GeoData();
            if (Geo.City == null)
                Geo.City = new CityGeoData();
            Geo.City.CountryCode = countryCode;
            Geo.City.RegionCode = regionCode;
            Geo.City.City = city;
            return this;
        }

        AppStatsEvent AppStatsEventExtensions.IAppStatsGeoContinuation<AppStatsEvent>.Point(string latitude, string longitude)
        {
            if (Geo == null)
                Geo = new GeoData();
            if (Geo.Point == null)
                Geo.Point = new PointGeoData();
            Geo.Point.Latitude = latitude;
            Geo.Point.Longitude = longitude;
            return this;
        }
    }
}