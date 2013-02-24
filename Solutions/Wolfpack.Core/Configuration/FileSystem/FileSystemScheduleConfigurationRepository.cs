using System;
using System.Collections.Generic;
using System.IO;
using Castle.Core.Internal;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Configuration.FileSystem
{
    public class FileSystemScheduleConfigurationRepository : FileSystemConfigurationRepository
    {
        public FileSystemScheduleConfigurationRepository(string baseFolder) : base(baseFolder)
        {
        }

        protected override IEnumerable<FileSystemConfigurationEntry> ProcessEntries(IEnumerable<FileSystemConfigurationEntry> entries)
        {
            entries.ForEach(e =>
                                {
                                    var name = Path.GetFileNameWithoutExtension(e.FileInfo.Name);

                                    Logger.Debug("\tAdding type '{0}' named '{1}' to container...",
                                        e.Entry.ConcreteType, name); 
                                    var concreteType = Type.GetType(e.Entry.ConcreteType);
                                    var instance = Serialiser.FromJson(e.Entry.Data, concreteType);

                                    Container.RegisterInstance(instance, name);

                                    e.Entry.RequiredProperties
                                        .AddIfMissing(Tuple.Create(ConfigurationEntry.RequiredPropertyNames.Name, name));
                                });
            return entries;
        }

        public override void Save(ConfigurationChangeRequest change)
        {
            if (!change.Entry.Tags.ContainsAll(PluginTypes.Schedule))
                return;

            var filepath = Path.Combine(_baseFolder, Path.ChangeExtension(change.Entry.Name, ConfigFileExtension));

            if (!HandleChange(change, filepath))
                throw new InvalidOperationException(string.Format("Unknown ChangeRequest action '{0}'", change.Action));
        }
    }
}