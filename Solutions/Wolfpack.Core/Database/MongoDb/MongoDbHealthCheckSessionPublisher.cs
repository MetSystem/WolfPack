using System;
using Norm;
using Norm.Collections;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;
using Wolfpack.Core.Publishers;

namespace Wolfpack.Core.Database.MongoDb
{
    public class MongoDbHealthCheckSessionPublisher : PublisherBase, IHealthCheckSessionPublisher
    {
        protected readonly MongoDbConfiguration myConfig;

        public MongoDbHealthCheckSessionPublisher(MongoDbConfiguration config)
        {
            myConfig = config;

            Enabled = config.Enabled;
            FriendlyId = config.FriendlyId;
        }

        #region IHealthCheckSessionPublisher Members

        public void Consume(HealthCheckAgentStart message)
        {
            Publish(message);
        }

        #endregion

        public void Publish(HealthCheckAgentStart message)
        {
            AgentData agentData = MapMessageToAgentData(message);
            string mongoHost = "mongodb://" + myConfig.ServerName + ":" + myConfig.Port + "/" + myConfig.DatabaseName;

            using (IMongo db = Mongo.Create(mongoHost))
            {
                IMongoCollection<AgentData> collection = db.GetCollection<AgentData>(myConfig.CollectionName);

                collection.Save(agentData);
            }
        }

        private AgentData MapMessageToAgentData(HealthCheckAgentStart message)
        {
            string data = SerialisationHelper<HealthCheckAgentStart>.DataContractSerialize(message);

            return new AgentData
                       {
                           //AgentId = message.Agent.AgentId,
                           //Data = data,
                           //EventType = "SessionStart",
                           //GeneratedOnUtc = message.DiscoveryStarted,
                           //ReceivedOnUtc = DateTime.UtcNow,
                           //SiteId = message.Agent.SiteId,
                           //Version = message.Id
                       };
        }
    }
}