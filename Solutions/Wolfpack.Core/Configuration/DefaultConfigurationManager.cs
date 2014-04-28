using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using System.Linq;

namespace Wolfpack.Core.Configuration
{
    public class DefaultConfigurationManager : IConfigurationManager
    {
        private static readonly object SyncObj = new object();

        private readonly IList<ConfigurationChangeRequest> _pendingChanges;
        private readonly IEnumerable<IConfigurationRepository> _repositories;
        private readonly AgentConfiguration _agentInfo;
        private IEnumerable<ConfigurationEntry> _entries;
        private bool _restartPending;

        public IEnumerable<ConfigurationChangeRequest> PendingChanges
        {
            get { return _pendingChanges; }
        }

        public DefaultConfigurationManager(IEnumerable<IConfigurationRepository> repositories,
            AgentConfiguration agentInfo)
        {
            _repositories = repositories;
            _agentInfo = agentInfo;
            _pendingChanges = new List<ConfigurationChangeRequest>();
        }

        public void Initialise()
        {
            var entries = new List<ConfigurationEntry>();
            entries.AddRange(LoadConfiguration());
            entries.AddRange(LoadCatalogue());
            _entries = entries.ToList();
        }

        public ConfigurationCatalogue GetCatalogue(params string[] tags)
        {
            var items = _entries.Where(ce => ce.Tags.ContainsAny(tags));
            return new ConfigurationCatalogue
            {
                InstanceId = _agentInfo.InstanceId,
                Items = items, 
                Pending = _pendingChanges
            };
        }

        public void Save(ConfigurationChangeRequest update)
        {
            lock(SyncObj)
            {
                if (_restartPending)
                    throw new InvalidOperationException(string.Format("Restart in progress"));

                update.Entry.Tags.RemoveAll(SpecialTags.ThatShouldNotBePersisted);
                _pendingChanges.Add(update);
            }
        }

        public void ApplyPendingChanges(bool restart)
        {
            lock (SyncObj)
            {
                if (!PendingChanges.Any())
                    return;

                Parallel.ForEach(_repositories, repository => PendingChanges.ToList().ForEach(repository.Save));
                _pendingChanges.Clear();

                if (!restart)
                    return;

                HandleRestart();
            }
        }

        protected void HandleRestart()
        {
            _restartPending = true;

            SystemCommand command;

            if (Environment.UserInteractive)
            {
                // console
                command = new SystemCommand
                              {
                                  RestartConsole = new RestartConsoleInstruction
                                                       {
                                                           ProcessId = Process.GetCurrentProcess().Id
                                                       }
                              };

                Logger.Debug("Writing console restart instruction file");
            }
            else
            {
                // service
                command = new SystemCommand
                              {
                                  RestartService = new RestartServiceInstruction
                                                       {
                                                           ServiceName = "WolfpackAgent"
                                                       }
                              };
                Logger.Debug("Writing service restart instruction file");
            }

            Serialiser.ToXmlInFile(SystemCommand.Filename, command);

            Logger.Info("Launching Wolfpack Helper to manage system restart...");
            Process.Start(new ProcessStartInfo("wolfpack.manager.exe")
                              {
                                  //WindowStyle = ProcessWindowStyle.Hidden
                              });
        }

        public void DiscardPendingChanges()
        {
            lock (SyncObj)
            {
                _pendingChanges.Clear();    
            }            
        }

        public IEnumerable<TagCloudEntry> GetTagCloud()
        {
            return _entries.SelectMany(e => e.Tags).GroupBy(t => t)
                .Select(g => new TagCloudEntry
                                 {
                                     Tag = g.Key, 
                                     Count = g.Count()
                                 }).OrderBy(t => t.Tag);
        }

        private IEnumerable<ConfigurationEntry> LoadConfiguration()
        {
            return _repositories.SelectMany(repository => repository.Load())
                .Select(ce =>
                            {
                                ce.Tags.AddIfMissing("Running");
                                return ce;
                            })
                .ToList();
        }

        private IEnumerable<ConfigurationEntry> LoadCatalogue()
        {
            Type[] items;

            TypeDiscovery.Discover<ISupportConfigurationDiscovery>(out items);

            return items.Select(i =>
                                    {
                                        var target = (ISupportConfigurationDiscovery) Activator.CreateInstance(i);
                                        return target.GetConfigurationMetadata();
                                    }).ToList();
        }
    }
}