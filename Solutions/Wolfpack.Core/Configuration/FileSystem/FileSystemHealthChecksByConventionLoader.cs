using System;
using System.Collections.Generic;
using System.IO;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using System.Linq;

namespace Wolfpack.Core.Configuration.FileSystem
{
    public class FileSystemHealthChecksByConventionLoader : FileSystemConfigurationRepository
    {
        public FileSystemHealthChecksByConventionLoader(string baseFolder)
            : base(baseFolder)
        {
        }

        protected override IEnumerable<FileSystemConfigurationEntry> ProcessEntries(IEnumerable<FileSystemConfigurationEntry> entries)
        {
            var validEntries = entries.Select(
                e =>
                    {
                        var shouldLoad = true;
                        if (e.FileInfo.Directory.FullName.Equals(_baseFolder,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            shouldLoad = false;

                            Logger.Warning(Logger.Event.During("FileSystemHealthChecksByConventionLoader.ProcessEntries")
                                .Description("HealthCheck configuration file '{0}' is not following convention - it should be located in a schedule-named sub folder of {1}. ** Configuration has not been loaded **",
                                e.FileInfo.FullName, _baseFolder));
                        }

                        return new
                        {
                            ShouldLoad = shouldLoad,
                            Entry = e
                        };
                    })
                .Where(es => es.ShouldLoad)
                .Select(es => es.Entry).ToList();


            validEntries.ForEach(
                e =>
                    {
                        var name = Path.GetFileNameWithoutExtension(e.FileInfo.Name);
                        var checkConfigType = Type.GetType(e.Entry.ConcreteType);
                        var checkConfig = Serialiser.FromJson(e.Entry.Data, checkConfigType);

                        if ((checkConfig is ICanBeSwitchedOff) && !((ICanBeSwitchedOff)checkConfig).Enabled)
                        {
                            // disabled plugin
                            return;
                        }

                        var checkTypeName = checkConfigType.Name.Replace("Config", string.Empty);

                        Type checkType;
                        if (!GetType<IHealthCheckPlugin>(checkTypeName, out checkType))
                            throw new InvalidOperationException(
                                string.Format("Searching for type name '{0}'; found no matches. Check the name of the folder", checkTypeName));
                        var check = Activator.CreateInstance(checkType, checkConfig);



                        var scheduleConfigName = e.FileInfo.Directory.Name;
                        var schedulerConfig = Container.Resolve(scheduleConfigName);
                        var schedulerTypeName = schedulerConfig.GetType().Name.Replace("Config", string.Empty);

                        Type schedulerType;
                        if (!GetType<IHealthCheckSchedulerPlugin>(schedulerTypeName, out schedulerType))
                            throw new InvalidOperationException(
                                string.Format(
                                    "Searching for type name '{0}'; found no matches. Check the name of the folder",
                                    schedulerTypeName));



                        var scheduler = Activator.CreateInstance(schedulerType, check, schedulerConfig) as
                                        IHealthCheckSchedulerPlugin;



                        Logger.Debug("\tAdding Scheduler Instance '{0}' running HealthCheck '{1}' named '{2}' to container...",
                            scheduler.GetType().Name, checkTypeName, name);
                        Container.RegisterInstance(scheduler, name);

                        e.Entry.RequiredProperties.AddIfMissing(
                            Tuple.Create(ConfigurationEntry.RequiredPropertyNames.Scheduler, scheduleConfigName),
                            Tuple.Create(ConfigurationEntry.RequiredPropertyNames.Name, name));
                    });

            return validEntries;
        }

        public override void Save(ConfigurationChangeRequest change)
        {
            if (!change.Entry.Tags.ContainsAll(PluginTypes.HealthCheck))
                return;

            string scheduler;

            if (!change.Entry.RequiredProperties.TryGetValue(ConfigurationEntry.RequiredPropertyNames.Scheduler, out scheduler))
                throw new InvalidOperationException(string.Format("Unable to update configuration, '{0}' property is missing",
                    ConfigurationEntry.RequiredPropertyNames.Scheduler));

            var filepath = Path.Combine(_baseFolder, scheduler, Path.ChangeExtension(change.Entry.Name, ConfigFileExtension));

            if (!HandleChange(change, filepath))
                throw new InvalidOperationException(string.Format("Unknown ChangeRequest action '{0}'", change.Action));
        }
    }
}