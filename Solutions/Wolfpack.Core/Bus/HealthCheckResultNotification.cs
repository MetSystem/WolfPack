using Wolfpack.Core.Interfaces.Entities;
using NServiceBus;

namespace Wolfpack.Core.Bus
{
    public class HealthCheckResultNotification : HealthCheckResult, IMessage
    {
    }
}