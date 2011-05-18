using Wolfpack.Core.Checks;
using Wolfpack.Tests.Bdd;

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