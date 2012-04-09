
using Fluent.IO;
using Wolfpack.Core.Containers;
using Wolfpack.Core.Testing.Bdd;

namespace Wolfpack.Core.Testing.Domains
{
    public class ContainerDomain : BddTestDomain
    {
        protected IContainer _container;

        public override void Dispose()
        {
            
        }

        public ContainerDomain()
        {
            TheConfigFolderIsCleaned();
        }

        private void TheConfigFolderIsCleaned()
        {
            Path.Get("Config").Delete(true);
        }

        public void TheComponentConfig_IsUsed(string configFile)
        {
            var filename = Path.Get(configFile).FileName;
            Path.Get(configFile).Copy(@"Config\" + filename, Overwrite.Always);
        }
    }
}