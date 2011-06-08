using System;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Tests.Bdd;
using Wolfpack.Tests.Drivers;

namespace Wolfpack.Tests.Checks
{
    public class StreamThresholdBasedDomainConfig : StreamThresholdCheckConfigBase
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

        public static StreamThresholdBasedDomainConfig FiresValues(params double[] values)
        {
            return new StreamThresholdBasedDomainConfig
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
        public StreamThresholdBasedDomainConfig AtInterval(int interval)
        {
            Interval = interval;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cycles"></param>
        /// <returns></returns>
        public StreamThresholdBasedDomainConfig Repeat(int cycles)
        {
            Cycles = cycles;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>      
        /// <returns></returns>
        public StreamThresholdBasedDomainConfig StreamingIsEnabled()
        {
            StreamData = true;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>      
        /// <returns></returns>
        public StreamThresholdBasedDomainConfig ThresholdIs(double threshold)
        {
            Threshold = threshold;
            return this;
        }
    }

    public class StreamThresholdBasedDomain : HealthCheckDomain
    {
        protected StreamThresholdBasedDomainConfig myConfig;

        public StreamThresholdBasedDomain(StreamThresholdBasedDomainConfig config)
        {
            myConfig = config;
        }

        public void TheCheckComponent()
        {
            HealthCheck = new AutomationHealthCheck(myConfig);
            HealthCheck.Initialise();
        }
    }
}