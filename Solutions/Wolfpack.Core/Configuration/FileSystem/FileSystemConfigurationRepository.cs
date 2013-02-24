using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ServiceStack.Text;
using Wolfpack.Core.Checks;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Schedulers;

namespace Wolfpack.Core.Configuration.FileSystem
{    
    public abstract class FileSystemConfigurationRepository : IConfigurationRepository
    {
        public const string ConfigFileExtension = "config";

        protected string _baseFolder;

        protected FileSystemConfigurationRepository(string baseFolder)
        {
            _baseFolder = SmartLocation.GetLocation(baseFolder);
        }
       
        protected abstract IEnumerable<FileSystemConfigurationEntry> ProcessEntries(IEnumerable<FileSystemConfigurationEntry> entries);

        public abstract void Save(ConfigurationChangeRequest change);

        public virtual IEnumerable<ConfigurationEntry> Load()
        {
            if (!Directory.Exists(_baseFolder))
                return new ConfigurationEntry[0];

            Logger.Info("Scanning for configuration entries in '{0}'...", _baseFolder);
            var files = Directory.GetFiles(_baseFolder, "*." + ConfigFileExtension, SearchOption.AllDirectories).ToList();

            //CreateTemplateScheduleJsonFile();
            //CreateTemplateHealthCheckConfigJsonFile();

            var entries = new List<FileSystemConfigurationEntry>();
            entries.AddRange(files.Select(
                filename =>
                    {
                        try
                        {
                            return new FileSystemConfigurationEntry
                            {
                                Entry = Serialiser.FromJsonInFile<ConfigurationEntry>(filename),
                                FileInfo = new FileInfo(filename)
                            };
                        }
                        catch (Exception e)
                        {
                            var ex = new InvalidOperationException(string.Format("Processing '{0}'", filename), e);
                            Logger.Error(Logger.Event.During("FileSystemScheduleConfigurationRepository.Load()").Encountered(ex));
                            return null;
                        }
                    })
                    .Where(e => e != null));

            var validEntries = ProcessEntries(entries);
            return validEntries.Select(e => e.Entry);
        }

        
        private void CreateTemplateHealthCheckConfigJsonFile()
        {
            var bob = new WmiProcessRunningCheckConfig
                          {
                              ProcessName = "notepad.exe",
                              Enabled = true,
                              FriendlyId = "IsNotepadRunning",
                              NotificationMode = "FailureOnly",
                              RemoteMachineId = "localhost"
                          };

            var fred = new ConfigurationEntry
                           {
                               ConcreteType = bob.GetType().AssemblyQualifiedName,
                               Data = Serialiser.ToJson(bob),
                               Tags = new List<string> { "HealthCheck", "WMI" }
                           };

            var filename = Path.Combine(_baseFolder, "notepad-running.config");
            Serialiser.ToJsonInFile(filename, fred);
        }

        private void CreateTemplateScheduleJsonFile()
        {
            var bob = new HealthCheckTwentyFourSevenSchedulerConfig
                          {
                              Weekdays = "9:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00"
                          };

            var fred = new ConfigurationEntry
                           {
                               ConcreteType = bob.GetType().AssemblyQualifiedName,
                               Data = bob.SerializeToString(),
                               Tags = new List<string> { "Scheduler" }
                           };

            using (var sw = new StreamWriter(Path.Combine(_baseFolder, "weekdays9to5.config")))
            {
                sw.Write(fred.SerializeToString());
            }
        }

        protected static bool GetType<T>(string targetTypeName, out Type targetType)
        {
            targetType = null;
            Type[] matchingTypes;

            if (!TypeDiscovery.Discover<T>(t => t.Name.Equals(targetTypeName, StringComparison.OrdinalIgnoreCase), 
                out matchingTypes))
                return false;

            if (matchingTypes.Count() != 1)
                throw new InvalidOperationException(string.Format("Searching for type '{0}' named '{1}'; found {2} matches, expected only 1",
                    typeof(T).Name,
                    targetTypeName,
                    matchingTypes.Count()));

            targetType = matchingTypes.First();
            return true;
        }

        protected static void WriteConfigFile(ConfigurationEntry entry, string filepath)
        {
            string newname;

            if (!entry.RequiredProperties.TryGetValue(ConfigurationEntry.RequiredPropertyNames.Name, out newname))
                throw new InvalidOperationException(string.Format("Unable to update configuration, '{0}' property is missing",
                    ConfigurationEntry.RequiredPropertyNames.Name));

            // detect a name change...
            if (!string.IsNullOrWhiteSpace(newname) && !newname.Equals(entry.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                File.Delete(filepath);

                var folder = Path.GetDirectoryName(filepath) ?? string.Empty;
                var ext = Path.GetExtension(filepath);
                filepath = Path.Combine(folder, Path.ChangeExtension(newname, ext));

                if (File.Exists(filepath))
                    throw new InvalidOperationException(string.Format(
                        "Unable to rename configuration, entry with name '{0}' already exists", newname));

                entry.Name = newname;
            }
          
            Serialiser.ToJsonInFile(filepath, entry);
        }

        protected bool HandleChange(ConfigurationChangeRequest change, string filepath)
        {
            switch (change.Action.ToLowerInvariant())
            {
                case ConfigurationActions.Delete:
                    File.Delete(filepath);
                    return true;

                case ConfigurationActions.Update:
                    WriteConfigFile(change.Entry, filepath);
                    return true;
            }

            return false;
        }
    }
}