namespace Wolfpack.Core.Geo
{
    using Interfaces;
    using Interfaces.Entities;

    /// <summary>
    /// This will return a preset geo data object
    /// </summary>
    public class StaticGeoLocator : IGeoLocator
    {
        protected readonly string myLongitude;
        protected readonly string myLatitude;

        public StaticGeoLocator(AgentConfiguration config)
        {
            myLongitude = config.Longitude;
            myLatitude = config.Latitude;
        }

        public GeoData Locate()
        {
            if (string.IsNullOrWhiteSpace(myLongitude) && string.IsNullOrWhiteSpace(myLatitude))
                return null;

            return new GeoData
                       {
                           Latitude = myLatitude,
                           Longitude = myLongitude
                       };
        }
    }
}