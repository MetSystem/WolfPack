using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Configuration
{
    public class ConfigurationManagerBootstrapper : IStartupPlugin
    {
        public Status Status { get; set; }

        public void Initialise()
        {
            if (!Container.IsRegistered<IConfigurationManager>())
            {
                Container.RegisterAsSingleton<IConfigurationManager>(typeof (DefaultConfigurationManager));
            }

            ConfigurationManager.Initialise(Container.Resolve<IConfigurationManager>());
        }
    }
}