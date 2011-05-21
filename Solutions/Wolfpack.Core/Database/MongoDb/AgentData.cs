using System;
using Norm;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Database.MongoDb
{
    [Serializable]
    public class AgentData : HealthCheckAgentStart
    {
        [MongoIdentifier]
        public int? MongoId { get; set; }
    }
}