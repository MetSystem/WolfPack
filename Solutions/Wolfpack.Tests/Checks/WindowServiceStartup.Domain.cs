using Wolfpack.Core.Checks;
using Wolfpack.Core.Testing.Domains;

namespace Wolfpack.Tests.Checks
{
    public class WindowServiceStartupDomain : HealthCheckDomain
    {
        public void TheCheckComponent(WindowsServiceStartupCheckConfig config)
        {
            HealthCheck = new WindowsServiceStartupCheck(config);
            HealthCheck.Initialise();
        }
    }
}