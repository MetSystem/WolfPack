using System;
using System.IO;
using Wolfpack.Core.Checks;
using Wolfpack.Core.Interfaces.Entities;
using Castle.Core;

namespace Wolfpack.Core.Bridge.FileSystem
{
    public class FileSystemBridgeCheckConfig : PluginConfigBase
    {
        public string QueueFolder { get; set; }
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
            Directory.GetFiles(myConfig.QueueFolder).ForEach(filename =>
                                                                 {
                                                                     string data;

                                                                     using (var sr = new StreamReader(filename))
                                                                     {
                                                                         data = sr.ReadToEnd();
                                                                     }

                                                                     var result =
                                                                         SerialisationHelper<HealthCheckResult>.
                                                                             DataContractDeserialize(data);

                                                                     //Publish();
                                                                     File.Delete(filename);
                                                                 });
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
            {
                Description = string.Format("Publishes file queue from folder {0}", myConfig.QueueFolder),
                TypeId = new Guid("95F2CEB7-F49F-46A6-917F-40744CAA4C17"),
                Name = myConfig.FriendlyId
            };
        }
    }
}