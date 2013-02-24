using System;
using Wolfpack.Core;
using Wolfpack.Core.Database.SQLite;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Contrib.Owl
{
    public class OwlEnergyMonitorCheckConfig : PluginConfigBase, ISupportNotificationMode
    {
        public string ConnectionString { get; set; }
        public string NotificationMode { get; set; }
    }

    public class OwlEnergyMonitorCheck : IHealthCheckPlugin
    {
        protected readonly OwlEnergyMonitorCheckConfig _config;
        protected PluginDescriptor _identity;
        protected DateTime _lastRun;

        public OwlEnergyMonitorCheck(OwlEnergyMonitorCheckConfig config)
        {
            _config = config;

            _identity = new PluginDescriptor
            {
                Description = string.Format("Reports energy information from your Owl Monitor Database"),
                TypeId = new Guid("55728986-4638-4b9c-8C8A-FF9F4290F564"),
                Name = _config.FriendlyId
            };

            _lastRun = DateTime.Now;
        }

        public void Initialise()
        {
            //myLastRun = new DateTime(2010, 11, 23,23,30,00);
        }

        public Status Status { get; set; }

        public PluginDescriptor Identity
        {
            get { return _identity; }
        }

        public void Execute()
        {
            var lastRun = Convert.ToInt64(_lastRun.ToString("yyyMMddHHmm"));

            using (var query = SQLiteAdhocCommand.UsingSmartConnection(_config.ConnectionString)
                .WithSql(SQLiteStatement.Create("select s.name, datetime(year || '-' || substr('0' || month, -2,2) || '-' || substr('0' || day, -2,2) || ' ' || substr('0' || hour, -2,2) || ':' || substr('0' || min, -2,2)) [recorded], ")
                .Append("ch1_amps_min, ch1_amps_avg, ch1_amps_max, ch1_kw_min, CAST(ch1_kw_avg AS REAL) [ch1_kw_avg], ch1_kw_max")
                .Append("from energy_history h join energy_sensor s on h.addr = s.addr")
                .Append("where CAST((year || substr('0' || month, -2,2) || substr('0' || day, -2,2) || substr('0' || hour, -2,2) || substr('0' || min, -2,2)) AS INTEGER) > ")
                .InsertParameter("@lastrun", lastRun)))
            {
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // assumes that results are in ascending order
                        _lastRun = Convert.ToDateTime(reader["recorded"]);

                        var sensor = reader["name"].ToString();
                        var data = HealthCheckData.For(Identity, _config.NotificationMode, sensor)
                            .AddTag(sensor)
                            // average watts
                            .ResultCountIs(Convert.ToDouble(reader["ch1_kw_avg"]))
                            .SetGeneratedOnUtc(_lastRun.ToUniversalTime());

                        Messenger.Publish(NotificationRequest.AlwaysPublish(data));

                        Logger.Debug("[{0}] Sent reading {1}kw taken {2}", 
                            Identity.Name,
                            data.ResultCount,
                            data.GeneratedOnUtc.ToLocalTime());
                    }
                }
            }
        }
    }
}
