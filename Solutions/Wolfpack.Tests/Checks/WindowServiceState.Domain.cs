using Wolfpack.Core.Checks;
using Wolfpack.Tests.Bdd;

namespace Wolfpack.Tests.Checks
{   
    public class WindowServiceStateDomain : HealthCheckDomain
    {
        public void TheCheckComponent(WindowsServiceStateCheckConfig config)
        {
            HealthCheck = new WindowsServiceStateCheck(config);
            HealthCheck.Initialise();
        }
    }
}