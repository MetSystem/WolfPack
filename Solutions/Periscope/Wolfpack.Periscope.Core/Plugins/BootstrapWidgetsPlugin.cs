using System.Collections.Generic;
using Wolfpack.Periscope.Core.Interfaces;
using System.Linq;

namespace Wolfpack.Periscope.Core.Plugins
{
    /// <summary>
    /// </summary>
    public class BootstrapWidgetsPlugin : IDashboardPlugin
    {
        private readonly IDashboardInfrastructure _infrastructure;
        private readonly IDashboardConfigurationRepository _repository;

        private IEnumerable<IWidgetBootstrapper> _bootstrappers;

        public BootstrapWidgetsPlugin(IDashboardInfrastructure infrastructure,
            IDashboardConfigurationRepository repository)
        {
            _infrastructure = infrastructure;
            _repository = repository;
        }

        public void Initialise()
        {
            var config = _repository.Load();
            _bootstrappers = config.Panels.SelectMany(p => p.Widgets)
                .Where(w => w.Bootstrapper != null)
                .Select(w => w.Bootstrapper);
        }

        public void Start()
        {
            foreach (var bootstrapper in _bootstrappers)
            {
                _infrastructure.Logger.LogDebug("Bootstrapping {0}...", bootstrapper.GetType().Name);
                bootstrapper.Execute();
            }
        }

        public void Dispose()
        {
            foreach (var bootstrapper in _bootstrappers)
            {
                bootstrapper.Dispose();
            }
        }
    }
}