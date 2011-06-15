using System;
using System.IO;
using Wolfpack.Core.Checks;
using Wolfpack.Core.Interfaces.Entities;
using Castle.Core;

namespace Wolfpack.Core.Bridge.FileSystem
{
    public class FileSystemBridgeCheckConfig : PluginConfigBase
    {
        public string Folder { get; set; }
    }

    public class FileSystemBridgeCheck : HealthCheckBase<FileSystemBridgeCheckConfig>
    {
        /// <summary>
        /// default ctor
        /// </summary>
        public FileSystemBridgeCheck(FileSystemBridgeCheckConfig config)
            : base(config)
        {
            
        }

        public override void Execute()
        {
            Directory.GetFiles(myConfig.Folder).ForEach(filename =>
                                                                 {
                                                                     string data;

                                                                     Logger.Debug("FileSystemBridge: Processing file '{0}'", filename);
                                                                     using (var sr = new StreamReader(filename))
                                                                     {
                                                                         data = sr.ReadToEnd();
                                                                     }
                                                                   
                                                                     switch (Path.GetFileName(filename)[0])
                                                                     {
                                                                         case 'S':
                                                                             Messenger.Publish(SerialisationHelper<HealthCheckAgentStart>.
                                                                                     DataContractDeserialize(data));
                                                                             break;
                                                                         case 'R':
                                                                             Messenger.Publish(SerialisationHelper<HealthCheckResult>.
                                                                                     DataContractDeserialize(data));
                                                                             break;
                                                                     }
                                                                     File.Delete(filename);
                                                                 });
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
            {
                Description = string.Format("Publishes file queue from folder {0}", myConfig.Folder),
                TypeId = new Guid("95F2CEB7-F49F-46A6-917F-40744CAA4C17"),
                Name = myConfig.FriendlyId
            };
        }
    }
}