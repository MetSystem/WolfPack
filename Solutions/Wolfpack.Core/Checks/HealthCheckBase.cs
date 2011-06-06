using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Checks
{
    public abstract class HealthCheckBase : IHealthCheckPlugin
    {
        private PluginDescriptor myIdentity;

        public Status Status { get; set; }

        public virtual void Initialise()
        {
            // do nothing
        }

        public PluginDescriptor Identity
        {
            get { return myIdentity ?? (myIdentity = BuildIdentity()); }
        }

        public abstract void Execute();

        protected abstract PluginDescriptor BuildIdentity();

        protected virtual void Publish(HealthCheckData message)
        {
            Messenger.Publish(message);
        }
    }
}