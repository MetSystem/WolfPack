using System;
using System.Diagnostics;
using System.Globalization;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Database.SqlServer;
using Wolfpack.Core.Notification;

namespace Wolfpack.Core.Checks
{
    public class ScalarCheckConfigBase : PluginConfigBase, ISupportNotificationMode
    {
        /// <summary>
        /// The portion of the query that starts at the FROM statement. This will 
        /// be prefixed with SELECT COUNT(*) to ensure the query returns a scalar
        /// result.
        /// </summary>
        public string FromQuery { get; set; }

        /// <summary>
        /// By default, zero rows returned from a query would be interpretted
        /// as a success (eg: you are looking for critial applications errors
        /// in the event log). If you are expecting rows to be returned then
        /// set this to true should no rows be returned.
        /// </summary>
        public bool InterpretZeroRowsAsAFailure { get; set; }

        public string NotificationMode { get; set; }
    }
    public abstract class ScalarCheckBase<T> : HealthCheckBase<T>
        where T : ScalarCheckConfigBase
    {
        protected readonly ScalarCheckConfigBase _baseConfig;
        protected PluginDescriptor  _identity;

        /// <summary>
        /// default ctor
        /// </summary>
        protected ScalarCheckBase(T config) : base(config)
        {
            _baseConfig = config;
        }

        public override void Execute()
        {
            if (!_baseConfig.FromQuery.StartsWith("FROM ", StringComparison.InvariantCultureIgnoreCase))
                throw new FormatException("The FromQuery config property is badly formed; it must start with 'FROM'");
            ValidateConfig();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Create the query
            var query = SqlServerStatement.Create("SELECT COUNT(*) ")
                .Append(_baseConfig.FromQuery).ToString();

            // Execute the query
            var rowcount = RunQuery(query);

            stopwatch.Stop();

            // is this a good or bad result?
            var result = DecideResult(_baseConfig, rowcount);

            Publish(result, rowcount, stopwatch.Elapsed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract int RunQuery(string query);

        /// <summary>
        /// Allows a subclass to validate its config properties
        /// </summary>
        protected virtual void ValidateConfig()
        {
            // do nothing
        }

        /// <summary>
        /// Implements logic to decide whether this query produces a successful or not result
        /// </summary>
        /// <param name="config"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        protected virtual bool DecideResult(ScalarCheckConfigBase config, int rowcount)
        {
            bool result;

            if (_baseConfig.InterpretZeroRowsAsAFailure)
            {
                result = (rowcount > 0);
            }
            else
            {
                result = (rowcount == 0);
            }

            return result;
        }

        /// <summary>
        /// Override this if you want to alter the publishing behaviour
        /// </summary>
        /// <param name="result"></param>
        /// <param name="rowcount"></param>
        /// <param name="duration"></param>
        protected virtual void Publish(bool result, int rowcount, TimeSpan duration)
        {
            var data = HealthCheckData.For(Identity, "{0} rows returned", rowcount)
                .ResultIs(result)
                .ResultCountIs(rowcount)
                .SetDuration(duration)
                .AddProperty("Rowcount", rowcount.ToString(CultureInfo.InvariantCulture))
                .AddProperty("Criteria", _baseConfig.FromQuery);

            Messenger.Publish(NotificationRequestBuilder.For(_config.NotificationMode, data).Build());
        }
    }
}