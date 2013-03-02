using Wolfpack.Core;
using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces;

namespace Wolfpack.Agent.Profiles
{
    /// <summary>
    /// Provides a common profile base functionality - this primarily
    /// consists of help constructing and registering publishers into
    /// the IoC container
    /// </summary>
    public abstract class ProfileBase : IRoleProfile
    {
        public abstract string Name { get; }
        public abstract void CustomiseRole();

        public IRolePlugin Role
        {
            get
            {
                ConfigurationManager.Initialise();
                Messenger.Initialise();                

                // load and execute all startup plugins
                // ...not using .SafeInitialise() method as
                // we want this to blow up loading the agent
                Container.RegisterAll<IStartupPlugin>()
                    .ResolveAll<IStartupPlugin>(c => c.InitialiseIfEnabled());

                // hook to create custom role components
                CustomiseRole();

                // finally resolve the role component
                return Container.Resolve<IRolePlugin>();
            }
        }
    }
}