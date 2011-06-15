using Wolfpack.Core.Interfaces;

namespace Wolfpack.Tests.Bdd
{
    public abstract class HealthCheckDomain : MessengerEnabledDomain
    {
        protected IHealthCheckPlugin HealthCheck { get; set; }


        public virtual void TheHealthCheckIsInvoked()
        {
            SafeExecute(() => HealthCheck.Execute());
        }
    }
}