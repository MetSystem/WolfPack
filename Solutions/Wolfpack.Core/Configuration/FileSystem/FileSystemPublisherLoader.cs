using System;
using System.Collections.Generic;
using System.IO;
using Castle.Core.Internal;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;

namespace Wolfpack.Core.Configuration.FileSystem
{
    public class FileSystemPublisherLoader : FileSystemConfigurationRepository
    {
        public FileSystemPublisherLoader(string baseFolder)
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

                        // probe for loading component by convention
                        var pluginTypeName = pluginConfigType.Name.Replace("Config", string.Empty);

                        Type pluginType;
                        if (GetType<INotificationEventPublisher>(pluginTypeName, out pluginType) &&
                            pluginType.GetConstructor(new[] { pluginConfigType }) != null)
                        {
                            // found it...
                            var plugin = (INotificationEventPublisher)Activator.CreateInstance(pluginType, pluginConfig);
                            Container.RegisterInstance(plugin, name);
                        }
                        else
                        {
                            // otherwise just register the configuration component, maybe
                            // another component (boostrapper?) will make use of it
                            Container.RegisterInstance(pluginConfigType, pluginConfig, name);
                        }

                        e.Entry.RequiredProperties.AddIfMissing(Tuple.Create(ConfigurationEntry.RequiredPropertyNames.Name, name));
                    });

            return entries;
        }

        public override void Save(ConfigurationChangeRequest change)
        {
            if (!change.Entry.Tags.ContainsAll(PluginTypes.Publisher))
                return;

            var filepath = Path.Combine(_baseFolder, Path.ChangeExtension(change.Entry.Name, ConfigFileExtension));

            if (!HandleChange(change, filepath))
                throw new InvalidOperationException(string.Format("Unknown ChangeRequest action '{0}'", change.Action));
        }
    }
}