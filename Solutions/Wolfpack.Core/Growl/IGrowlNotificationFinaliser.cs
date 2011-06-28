using Castle.DynamicProxy;
using Growl.Connector;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Growl
{
    public interface IGrowlNotificationFinaliser
    {
        void Finalise(HealthCheckResult result, Notification notification);
    }

    public interface IGrowlNotificationFinaliserInterceptor : IInterceptor
    {
    }
}