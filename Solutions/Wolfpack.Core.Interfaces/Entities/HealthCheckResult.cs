
using System;

namespace Wolfpack.Core.Interfaces.Entities
{
    public class HealthCheckResult
    {
        public static readonly DateTime BucketBaselineDate = new DateTime(2011, 01, 01);

        /// <summary>
        /// A unique identifier for this message
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The "type" of result - typically this is "Result" but other values
        /// in use are "AppStat". Use this property to identify 
        /// the type of event when selecting and querying the data once stored.
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// Provides a bucket for grouping by hour
        /// </summary>
        public int? HourBucket { get; set; }
        /// <summary>
        /// Provides a bucket for grouping by minute
        /// </summary>
        public int? MinuteBucket { get; set; }
        /// <summary>
        /// Provides a bucket for grouping by day
        /// </summary>
        public int? DayBucket { get; set; }

        /// <summary>
        /// Identifies the agent this result belongs to
        /// </summary>
        public AgentInfo Agent { get; set; }

        /// <summary>
        /// The health check information
        /// </summary>
        public HealthCheckData Check { get; set; }

        /// <summary>
        /// Default ctor
        /// </summary>
        public HealthCheckResult()
        {
            Id = Guid.NewGuid();
            EventType = "Result";

            MinuteBucket = (int)DateTime.UtcNow.Subtract(BucketBaselineDate).TotalMinutes;
            HourBucket = (int)DateTime.UtcNow.Subtract(BucketBaselineDate).TotalHours;
            DayBucket = (int)DateTime.UtcNow.Subtract(BucketBaselineDate).TotalDays;

        }

        /// <summary>
        /// Factory method to create results from health check data
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="data"></param>        
        /// <returns></returns>
        public static HealthCheckResult From(AgentInfo agent,
                                             HealthCheckData data)
        {
            return new HealthCheckResult
                       {
                           Agent = agent,
                           Check = data
                       };
        }
    }
}