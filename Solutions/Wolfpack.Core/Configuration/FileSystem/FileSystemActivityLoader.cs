using System;
using System.Collections.Generic;
using System.IO;
using Castle.Core.Internal;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Configuration.FileSystem
{
    public class FileSystemActivityLoader : FileSystemConfigurationRepository
    {
        public FileSystemActivityLoader(string baseFolder)
            : base(baseFolder)
        {
        }

        protected override IEnumerable<FileSystemConfigurationEntry> ProcessEntries(IEnumerable<FileSystemConfigurationEntry> entries)
        {
            entries.ForEach(
                e =>
                    {
                        var name = Path.GetFileNameWithoutExtension(e.FileInfo.Name);
                        var pluginConfigType = Type.GetType(e.Entry.ConcreteType);
                        var pluginConfig = Serialiser.FromJson(e.Entry.Data, pluginConfigType);

                        Container.RegisterInstance(pluginConfigType, pluginConfig, name);

                        e.Entry.RequiredProperties.AddIfMissing(Tuple.Create(ConfigurationEntry.RequiredPropertyNames.Name, name));
                    });

            return entries;
        }

        public override void Save(ConfigurationChangeRequest change)
        {
            if (!change.Entry.Tags.ContainsAll(PluginTypes.Activity))
                return;

            var filepath = Path.Combine(_baseFolder, Path.ChangeExtension(change.Entry.Name, ConfigFileExtension));

            if (!HandleChange(change, filepath))
                throw new InvalidOperationException(string.Format("Unknown ChangeRequest action '{0}'", change.Action));
        }
    }
}