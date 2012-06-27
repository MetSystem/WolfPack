using System;
using NUnit.Framework;
using StoryQ;
using Wolfpack.Core;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Testing.Bdd;

namespace Wolfpack.Tests.System
{
    public class EntitySpecs : BddFeature
    {
        protected override Feature DescribeFeature()
        {
            return new Story("Testing Wolfpack data entities")
                .InOrderTo("not break data entities")
                .AsA("component that interacts with the entities")
                .IWant("these tests to prove this is so!");
        }

        [TestCase("v2.4.0")]
        public void HealthCheckResultDeserialisePreviousVersion(string version)
        {
            using (var domain = new EntitiesDomain())
            {
                Feature.WithScenario("")
                    .Given(domain.TheHealthCheckResultDataFilenameIsBuiltForVersion_, version)
                    .When(domain.TheHealthCheckResultXmlIsDeserialised)
                    .Then(domain.ThrewNoException)
                        .And(domain.TheResultShouldNotBeNull)
                    .ExecuteWithReport();
            }
        }
    }

    public class EntitiesDomain : BddTestDomain
    {
        private string _xmlFilePath;
        private HealthCheckResult _resultEntity;

        public override void Dispose()
        {            
        }

        public void TheHealthCheckResultDataFilenameIsBuiltForVersion_(string version)
        {
            _xmlFilePath = string.Format(@"TestData\{0}\HealthCheckResult.xml", version);
        }

        public void TheHealthCheckResultXmlIsDeserialised()
        {
            SafeExecute(() => _resultEntity = SerialisationHelper<HealthCheckResult>.DataContractDeserializeFromFile(_xmlFilePath));
        }

        public void TheResultNotificationModeShouldBeNull()
        {
            Assert.That(_resultEntity.Check.NotificationMode, Is.Null);
        }

        public void TheResultShouldNotBeNull()
        {
            Assert.That(_resultEntity, Is.Not.Null);
        }
    }
}