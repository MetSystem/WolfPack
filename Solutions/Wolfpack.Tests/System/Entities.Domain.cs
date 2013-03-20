using NUnit.Framework;
using Wolfpack.Core;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Testing.Bdd;

namespace Wolfpack.Tests.System
{
    public class EntitiesDomain : BddTestDomain
    {
        private string _xmlFilePath;
        private NotificationEventHealthCheck _resultEntity;

        public override void Dispose()
        {            
        }

        public void TheHealthCheckResultDataFilenameIsBuiltForVersion_(string version)
        {
            _xmlFilePath = string.Format(@"TestData\{0}\HealthCheckResult.xml", version);
        }

        public void TheHealthCheckResultXmlIsDeserialised()
        {
            SafeExecute(() => _resultEntity = Serialiser.FromXmlInFile<NotificationEventHealthCheck>(_xmlFilePath));
        }

        public void TheResultShouldNotBeNull()
        {
            Assert.That(_resultEntity, Is.Not.Null);
        }
    }
}