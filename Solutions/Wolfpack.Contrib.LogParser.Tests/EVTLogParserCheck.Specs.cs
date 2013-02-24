using NUnit.Framework;
using StoryQ;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Testing.Bdd;
using Wolfpack.Core.Testing.Domains;

namespace Wolfpack.Contrib.LogParser.Tests
{
    [TestFixture]
    public class EVTLogParserCheckSpecs : BddFeature
    {
        protected override Feature DescribeFeature()
        {
            return new Story("Ensure the event log parser health check functions correctly")
                .InOrderTo("detect data problems in the event log")
                .AsA("operator concerned with system health")
                .IWant("this check to alert me when it detects conditions I have asked it to monitor")
                .Tag("logparser")
                .Tag("eventlog");
        }

        [Test]
        [TestCase(false, 0)]
        [TestCase(true, 1)]
        public void ArtifactFileCreation(bool generateArtifacts, int expectedFiles)
        {
            var check = new EVTLogParserCheck(new EVTLogParserCheckConfig
                                                  {
                                                      FriendlyId = "TestCheck",
                                                      Query = "select top 100 * from application",
                                                      Enabled = true,
                                                      GenerateArtifacts = generateArtifacts
                                                  });

            using (var domain = new LogParserCheckDomain(check))
            {
                Feature.WithScenario(string.Format("should {0}create an artifact file", generateArtifacts ? "" : "not "))
                    .Given(domain.TheCheckArtifactsArePurged, check.Identity.Name)
                        .And(domain.TheDefaultArtifactManagerIsLoaded)
                        .And(domain.TheAgentIsStarted)
                    .When(domain.TheHealthCheckIsInvoked)
                    .Then(domain.ThereShouldBe_ArtifactFilesForCheck_, expectedFiles, check.Identity.Name)
                    .ExecuteWithReport();
            }
        }

        [Test]
        [TestCase(false, false)]
        [TestCase(true, true)]
        public void CheckWithRowsGeneratesCorrectResultDependingUponInterpretZeroFlag(bool interpretZeroFlag, bool expectedResult)
        {
            var check = new EVTLogParserCheck(new EVTLogParserCheckConfig
                                                  {
                                                      FriendlyId = "TestCheck",
                                                      Query = "select top 10 * from application",
                                                      Enabled = true,
                                                      InterpretZeroRowsAsAFailure = interpretZeroFlag,
                                                      GenerateArtifacts = false
                                                  });

            using (var domain = new LogParserCheckDomain(check))
            {
                Feature.WithScenario("check with result rows creates a failure result message")
                    .Given(domain.TheCheckArtifactsArePurged, check.Identity.Name)
                        .And(domain.TheDefaultArtifactManagerIsLoaded)
                        .And(domain.TheAgentIsStarted)
                    .When(domain.TheHealthCheckIsInvoked)
                    .Then(domain.ThereShouldBe_HealthCheckNotificationsReceived, 1)
                        .And(domain.TheNotificationRecievedAtIndex_ShouldHaveResult_, 1, expectedResult)
                        .And(domain.TheNotificationReceivedAtIndex_ShouldHaveResultCount_, 1, 10d)
                    .ExecuteWithReport();
            }
        }

        [Test]
        public void CheckWithoutRowsGeneratesSuccessResultWhenInterpretZeroFlagSetToFalse()
        {
            var check = new EVTLogParserCheck(new EVTLogParserCheckConfig
                                                  {
                                                      FriendlyId = "TestCheck",
                                                      Query = "select top 10 * from application WHERE SourceName='sjkfskf34'",
                                                      Enabled = true,
                                                      InterpretZeroRowsAsAFailure = false,
                                                      GenerateArtifacts = false
                                                  });

            using (var domain = new LogParserCheckDomain(check))
            {
                Feature.WithScenario("check with result rows creates a failure result message")
                    .Given(domain.TheCheckArtifactsArePurged, check.Identity.Name)
                        .And(domain.TheDefaultArtifactManagerIsLoaded)
                        .And(domain.TheAgentIsStarted)
                    .When(domain.TheHealthCheckIsInvoked)
                    .Then(domain.ThereShouldBe_HealthCheckNotificationsReceived, 1)
                        // notifcation[0] is the agent start
                        .And(domain.TheNotificationRecievedAtIndex_ShouldHaveResult_, 1, true)
                        .And(domain.TheNotificationReceivedAtIndex_ShouldHaveResultCount_, 1, 0d)
                    .ExecuteWithReport();
            }
        }
    }

    public class LogParserCheckDomain : HealthCheckDomain
    {
        private readonly IHealthCheckPlugin _check;

        public LogParserCheckDomain(IHealthCheckPlugin check)
        {
            _check = check;
        }

        protected override IHealthCheckPlugin HealthCheck
        {
            get { return _check; }
        }
    }
}