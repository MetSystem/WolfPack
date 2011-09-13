using System;
using System.Configuration;
using System.IO;
using Castle.Core;
using Castle.Core.Resource;
using Castle.Windsor.Configuration.Interpreters;

namespace Wolfpack.Core.Containers
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>http://stackoverflow.com/questions/317981/can-castle-windsor-locate-files-in-a-subdirectory</remarks>
    public class ZeroAppConfigXmlInterpreter : XmlInterpreter
    {
        public override void ProcessResource(IResource source, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            // default stuff...
            base.ProcessResource(source, store);

            // custom stuff..auto register all config\*.castle.config files
            var configFilesLocation = SmartLocation.GetLocation("config");

            if (!Directory.Exists(configFilesLocation))
                return;

            ProcessFolder(store, configFilesLocation);
        }

        protected void ProcessFolder(Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store, string path)
        {
            foreach (var extraConfig in Directory.GetFiles(path, "*.castle.config"))
            {
                try
                {
                    var interpreter = new XmlInterpreter(extraConfig) { Kernel = Kernel };
                    interpreter.ProcessResource(interpreter.Source, store);
                }
                catch (ConfigurationErrorsException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to load configuration: " + extraConfig, ex);
                }
            }
            
            Directory.GetDirectories(path).ForEach(folder => ProcessFolder(store, folder));
        }
    }
}