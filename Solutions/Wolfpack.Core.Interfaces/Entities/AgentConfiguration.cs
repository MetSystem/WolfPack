using System;

namespace Wolfpack.Core.Interfaces.Entities
{
    public class AgentConfiguration
    {
        public string SiteId { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public AgentInfo BuildInfo()
        {
            return new AgentInfo
            {
                SiteId = Environment.MachineName,
                AgentId = SiteId
            };
        }
    }
}