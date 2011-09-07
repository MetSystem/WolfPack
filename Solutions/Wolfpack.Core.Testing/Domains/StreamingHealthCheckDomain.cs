using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Testing.Drivers;

namespace Wolfpack.Core.Testing.Domains
{
    public class StreamingHealthCheckDomainConfig : StreamThresholdCheckConfigBase
    {
        public double[] Values;
        /// <summary>
        /// The interval between firing each value
        /// </summary>
        public int Interval;
        /// <summary>
        /// The number of times to repeat firing of all the values
        /// </summary>
        public int Cycles;

        public static StreamingHealthCheckDomainConfig FiresValues(params double[] values)
        {
            return new StreamingHealthCheckDomainConfig
            {
                Values = values,
                Cycles = 1
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval">interval in milliseconds</param>
        /// <returns></returns>
        public StreamingHealthCheckDomainConfig AtInterval(int interval)
        {
            Interval = interval;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cycles"></param>
        /// <returns></returns>
        public StreamingHealthCheckDomainConfig Repeat(int cycles)
        {
            Cycles = cycles;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>      
        /// <returns></returns>
        public StreamingHealthCheckDomainConfig StreamingIsEnabled()
        {
            StreamData = true;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>      
        /// <returns></returns>
        public StreamingHealthCheckDomainConfig ThresholdIs(double threshold)
        {
            Threshold = threshold;
            return this;
        }
    }

    public class StreamingHealthCheckDomain : HealthCheckDomain
    {
        protected StreamingHealthCheckDomainConfig myConfig;

        public StreamingHealthCheckDomain(StreamingHealthCheckDomainConfig config)
        {
            myConfig = config;
        }

        public void TheCheckComponent()
        {
            HealthCheck = new AutomationStreamingHealthCheck(myConfig);
            HealthCheck.Initialise();
        }
    }
}