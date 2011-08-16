using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Interfaces
{
    public interface IHealthCheckPlugin : IPlugin
    {
        /// <summary>
        /// Used to identify a plugin
        /// </summary>
        PluginDescriptor Identity { get; }

        /// <summary>
        /// This is the health check code - it is triggered by the 
        /// scheduler that this check has been bound to
        /// </summary>
        void Execute();
    }

    /// <summary>
    /// This interface is used for HealthChecks to be loaded directly from the container.
    /// This is the recommended interface for HealthChecks to use
    /// </summary>
    public interface IHealthCheckPluginEx : IHealthCheckPlugin, ICanBeSwitchedOff
    {
    }

    /// <summary>
    /// NOTE: NO SUPPORT FOR THIS YET - JUST AN IDEA!
    /// This interface can optionally be implemented by an <see cref="IHealthCheckPlugin"/>
    /// component to read and write it's state immediately before and after it's invocation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHealthCheckStateManager<T>
    {
        T Load(PluginDescriptor check);
        void Save(PluginDescriptor check, T state);
    }
}