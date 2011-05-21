using System;
using Norm;
using Norm.Collections;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;
using Wolfpack.Core.Publishers;

namespace Wolfpack.Core.Database.MongoDb
{
    public class MongoDbHealthCheckResultPublisher : PublisherBase, IHealthCheckResultPublisher
    {
        protected readonly MongoDbConfiguration myConfig;

        public MongoDbHealthCheckResultPublisher(MongoDbConfiguration config)
        {
            myConfig = config;

            Enabled = config.Enabled;
            FriendlyId = config.FriendlyId;
        }

        #region IHealthCheckResultPublisher Members

        public void Consume(HealthCheckResult message)
        {
            Publish(message);
        }

        #endregion

        public void Publish(HealthCheckResult message)
        {
            AgentData agentData = MapMessageToAgentData(message);
            string mongoHost = "mongodb://" + myConfig.ServerName + ":" + myConfig.Port + "/" + myConfig.DatabaseName;

            using (IMongo db = Mongo.Create(mongoHost))
            {
                IMongoCollection<AgentData> collection = db.GetCollection<AgentData>(myConfig.CollectionName);

                collection.Save(agentData);
            }
        }

        private AgentData MapMessageToAgentData(HealthCheckResult message)
        {
            string data = SerialisationHelper<HealthCheckResult>.DataContractSerialize(message);

            return new AgentData
                       {
                           //AgentId = message.Agent.AgentId,
                           //CheckId = message.Check.Identity.Name,
                           //Data = data,
                           //EventType = "Result",
                           //GeneratedOnUtc = message.Check.GeneratedOnUtc,
                           //ReceivedOnUtc = DateTime.UtcNow,
                           //Result = message.Check.Result,
                           //ResultCount = message.Check.ResultCount,
                           //SiteId = message.Agent.SiteId,
                           //Tags = message.Check.Tags,
                           //TypeId = message.Check.Identity.TypeId,
                           //Version = message.Id
                       };
        }
    }
}