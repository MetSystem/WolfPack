using System;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Checks
{
    /// <summary>
    /// Use this as a base for a Health Check component that is registered directly with the 
    /// container (as opposed to one that is has its config component registered). Registering
    /// the Health Checks directly with the container is the recommended integrating route.
    /// </summary>
    public abstract class HealthCheckBaseEx : IHealthCheckPluginEx
    {
        private readonly PluginDescriptor myIdentity;
        
        public Status Status { get; set; }
        public bool Enabled { get; set; }

        protected HealthCheckBaseEx(string friendlyName,
            bool enabled,
            string description,
            Guid typeId)
        {
            Enabled = enabled;
            myIdentity = new PluginDescriptor
                             {
                                 Name = friendlyName,
                                 Description = description,
                                 TypeId = typeId
                             };
        }

        public virtual void Initialise()
        {
            // do nothing
        }

        public PluginDescriptor Identity
        {
            get { return myIdentity; }
        }

        public abstract void Execute();

        protected virtual void Publish(HealthCheckData message)
        {
            Messenger.Publish(message);
        }
    }

    /// <summary>
    /// This provides a base class for legacy style Health Checks that use the original
    /// mechanism of registering a config component with the container and then Wolfpack
    /// runtime would use a convention to infer the actual Health Check component to load.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class HealthCheckBase<T> : IHealthCheckPlugin
    {
        protected T myConfig;
        private PluginDescriptor myIdentity;

        protected HealthCheckBase(T config)
        {
            myConfig = config;
        }
        
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