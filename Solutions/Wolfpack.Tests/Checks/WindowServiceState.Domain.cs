using Wolfpack.Core.Checks;
using Wolfpack.Core.Testing.Domains;

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