using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Wcf
{
    public class WolfpackService : IWolfpack
    {
        public void CaptureAgentStart(HealthCheckAgentStart session)
        {
            Messenger.Publish(session);
        }

        public void CaptureResult(HealthCheckResult result)
        {
            Messenger.Publish(result);
        }
    }
}