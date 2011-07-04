using System;
using System.Diagnostics;
using Magnum;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.AppStats
{
    public class AppStatsEngine
    {
        private static AppStatsConfig myConfig;
        
        public static void Initialise(AppStatsConfig config)
        {
            myConfig = config;
        }

        public static void Publish(AppStatsEvent stat)
        {
            Guard.AgainstNull(stat);

            if (string.IsNullOrEmpty(stat.SiteId))
                stat.SiteId = myConfig.MachineId;
            if (string.IsNullOrEmpty(stat.AgentId))
                stat.AgentId = myConfig.ApplicationId;

            myConfig.Publisher.Publish(stat);
        }

        public static void One(string id)
        {
            Count(1, id);
        }

        public static void One(string id, string context, params object[] args)
        {
            Count(1, id, context, args);
        }

        public static void Count(double value, string id)
        {
            Count(value, id, string.Empty);
        }

        public static void Count(double value, string id, string context, params object[] args)
        {           
            myConfig.Publisher.Publish(new AppStatsEvent
            {
                SiteId = myConfig.MachineId,
                AgentId = myConfig.ApplicationId,
                CheckId = id,
                ResultCount = value,
                Tag = string.Format(context, args)
            });
        }

        public static AppStatsEventTimer Time()
        {
            return new AppStatsEventTimer();
        }

        public static AppStatsEventTimer Time(string id)
        {
            return new AppStatsEventTimer(id);
        }

        public static AppStatsEventTimer Time(string id, string context, params object[] args)
        {
            return new AppStatsEventTimer(id, context, args);
        }

        public class AppStatsEventTimer : AppStatsEvent,
            IDisposable,
            AppStatsEventExtensions.IAppStatsPieChartContinuation<AppStatsEventTimer>,
            AppStatsEventExtensions.IAppStatsGeoContinuation<AppStatsEventTimer>
        {
            private readonly Stopwatch myTimer;

            public AppStatsEventTimer()
            {
                SiteId = myConfig.MachineId;
                AgentId = myConfig.ApplicationId;
                myTimer = new Stopwatch();
                myTimer.Start();
            }

            public AppStatsEventTimer(string id)
                : this()
            {
                CheckId = id;
            }

            public AppStatsEventTimer(string id, string context, params object[] args)
                : this(id)
            {
                Tag = String.Format(context, args);
            }

            AppStatsEventTimer AppStatsEventExtensions.IAppStatsPieChartContinuation<AppStatsEventTimer>.Segment(string id)
            {
                Tag = id;
                return this;
            }

            AppStatsEventTimer AppStatsEventExtensions.IAppStatsGeoContinuation<AppStatsEventTimer>.IpAddress(string ipAddress)
            {
                if (Geo == null)
                    Geo = new GeoData();
                if (Geo.Dns == null)
                    Geo.Dns = new DnsGeoData();
                Geo.Dns.IpAddress = ipAddress;
                return this;
            }

            AppStatsEventTimer AppStatsEventExtensions.IAppStatsGeoContinuation<AppStatsEventTimer>.Hostname(string hostName)
            {
                if (Geo == null)
                    Geo = new GeoData();
                if (Geo.Dns == null)
                    Geo.Dns = new DnsGeoData();
                Geo.Dns.Hostname = hostName;
                return this;
            }

            AppStatsEventTimer AppStatsEventExtensions.IAppStatsGeoContinuation<AppStatsEventTimer>.City(string countryCode, string city)
            {
                if (Geo == null)
                    Geo = new GeoData();
                if (Geo.City == null)
                    Geo.City = new CityGeoData();
                Geo.City.CountryCode = countryCode;
                Geo.City.City = city;
                return this;
            }

            AppStatsEventTimer AppStatsEventExtensions.IAppStatsGeoContinuation<AppStatsEventTimer>.City(string countryCode, string regionCode, string city)
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

            AppStatsEventTimer AppStatsEventExtensions.IAppStatsGeoContinuation<AppStatsEventTimer>.Point(string latitude, string longitude)
            {
                if (Geo == null)
                    Geo = new GeoData();
                if (Geo.Point == null)
                    Geo.Point = new PointGeoData();
                Geo.Point.Latitude = latitude;
                Geo.Point.Longitude = longitude;
                return this;
            }

            public void Dispose()
            {
                myTimer.Stop();
                Time(myTimer.Elapsed);

                myConfig.Publisher.Publish(this);
            }
        }
    }
}