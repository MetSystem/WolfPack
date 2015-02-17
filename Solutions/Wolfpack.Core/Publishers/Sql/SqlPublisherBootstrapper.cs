using System;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Repositories.Sql;

namespace Wolfpack.Core.Publishers.Sql
{
    public class SqlPublisherBootstrapper : ISupportBootStrapping<SqlPublisherConfiguration>
    {
        public void Execute(SqlPublisherConfiguration config)
        {
            if (!config.Enabled)
                return;

            // some magic sauce to make the AttachDbFilename connectionstring property 
            // accept "relative path" filenames using the |DataDirectory| token
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);

            if (config.UseAsRepository)
            {
                var repository = new SqlRepository(new SqlDbContext(config.ConnectionName));
                Container.RegisterInstance<INotificationRepository>(repository);
            }
        }
    }
}