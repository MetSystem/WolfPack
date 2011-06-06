using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Checks
{
    public abstract class StreamThresholdCheckBase : HealthCheckBase
    {
        public bool StreamData { get; set; }
        public double? Threshold { get; set; }

        protected override void Publish(HealthCheckData message)
        {
            var breached = IsThresholdBreached(message.ResultCount);

            if (breached)
                AdjustMessageForBreach(message);

            if (StreamData || breached)
                base.Publish(message);
        }

        protected virtual void AdjustMessageForBreach(HealthCheckData message)
        {
            message.Result = false;
        }

        protected virtual bool IsThresholdBreached(double? resultCount)
        {
            if (!Threshold.HasValue)
                return false;
            if (!resultCount.HasValue)
                return false;
            return (resultCount.Value > Threshold.Value);
        }
    }
}