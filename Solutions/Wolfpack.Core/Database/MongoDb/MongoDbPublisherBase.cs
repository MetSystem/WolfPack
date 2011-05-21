using System.Linq;
using Norm;
using Norm.Collections;
using Norm.Protocol.Messages;
using Wolfpack.Core.Publishers;

namespace Wolfpack.Core.Database.MongoDb
{
    public class MongoDbPublisherBase : PublisherBase
    {
        protected readonly MongoDbConfiguration myConfig;

        public MongoDbPublisherBase(MongoDbConfiguration config)
        {
            myConfig = config;

            Enabled = config.Enabled;
            FriendlyId = config.FriendlyId;
        }

        public override void Initialise()
        {
            var mongoHost = "mongodb://" + myConfig.ServerName + ":" + myConfig.Port + "/" + myConfig.DatabaseName;

            using (var db = Mongo.Create(mongoHost))
            {
                var collections = db.Database.GetAllCollections().ToList();
                var exists = collections.Exists(x => x.Name == myConfig.DatabaseName + "." + myConfig.CollectionName);

                if (exists)
                    return;

                var result = db.Database.CreateCollection(new CreateCollectionOptions
                                                               {
                                                                   AutoIndexId = myConfig.AutoIndexId,
                                                                   Capped = myConfig.Capped,
                                                                   Create = myConfig.Create,
                                                                   Max = myConfig.Max,
                                                                   Name = myConfig.CollectionName,
                                                                   Size = myConfig.Size,
                                                               });

                var collection = db.GetCollection<AgentData>(myConfig.CollectionName);

                //collection.CreateIndex(a => a.,
                //                       "eventTypeIndex", true,
                //                       IndexOption.Ascending);
                //collection.CreateIndex(a => a.Result,
                //                       "resultIndex", true,
                //                       IndexOption.Ascending);
                //collection.CreateIndex(a => a.GeneratedOnUtc,
                //                       "generatedOnIndex", true,
                //                       IndexOption.Ascending);
                //collection.CreateIndex(a => a.CheckId,
                //                       "checkIdIndex", true,
                //                       IndexOption.Ascending);
            }
        }
    }
}