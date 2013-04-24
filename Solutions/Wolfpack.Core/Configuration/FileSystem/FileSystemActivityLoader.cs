using System;
using System.Collections.Generic;
using System.IO;
using Castle.Core.Internal;
using Wolfpack.Core.Interfaces;
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
                        var configType = Type.GetType(e.Entry.ConfigurationType);
                        var config = Serialiser.FromJson(e.Entry.Data, configType);

                        if (IsDisabled(config))
                            return;

                        var name = Path.GetFileNameWithoutExtension(e.FileInfo.Name);
                        Container.RegisterInstance(configType, config, name);
                        FindAndExecuteBootstrappers(configType, config);
                        e.Entry.RequiredProperties.AddIfMissing(Tuple.Create(ConfigurationEntry.RequiredPropertyNames.Name, name));

                        if (string.IsNullOrWhiteSpace(e.Entry.PluginType))
                            return;

                        var pluginType = Type.GetType(e.Entry.PluginType);
                        Container.RegisterAsSingleton<IActivityPlugin>(pluginType);
                    });

            return entries;
        }

        public override void Save(ConfigurationChangeRequest change)
        {
            if (!change.Entry.Tags.ContainsAll(SpecialTags.Activity))
                return;

            var filepath = Path.Combine(_baseFolder, Path.ChangeExtension(change.Entry.Name, ConfigFileExtension));

            if (!HandleChange(change, filepath))
                throw new InvalidOperationException(string.Format("Unknown ChangeRequest action '{0}'", change.Action));
        }
    }
}