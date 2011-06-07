using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Checks
{
    public abstract class StreamThresholdCheckBase<T> : HealthCheckBase 
        where T : StreamThresholdCheckConfigBase
    {
        protected T myConfig;

        protected override void Publish(HealthCheckData message)
        {
            var breached = IsThresholdBreached(message.ResultCount);

            if (breached)
                AdjustMessageForBreach(message);

            if (myConfig.StreamData || breached)
                base.Publish(message);
        }

        protected virtual void AdjustMessageForBreach(HealthCheckData message)
        {
            message.Result = false;
        }

        protected virtual bool IsThresholdBreached(double? resultCount)
        {
            if (!myConfig.Threshold.HasValue)
                return false;
            if (!resultCount.HasValue)
                return false;
            return (resultCount.Value > myConfig.Threshold.Value);
        }
    }
}