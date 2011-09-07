using NUnit.Framework;
using StoryQ;
using Wolfpack.Core.Testing.Bdd;

namespace Wolfpack.Tests.BuildAnalytics
{
    [TestFixture]
    public class StoryQXmlReportParserSpecs : BddFeature
    {
        protected override Feature DescribeFeature()
        {
            return new Story("Be able to consume the statistcs from a StoryQ report")
                .InOrderTo("Visualise and monitor storyq information")
                .AsA("Sys Admin")
                .IWant("To be able to provide a report and have it parsed into health check result messages");
        }

        [Test]
        public void NotZippedReportInSetLocation()
        {
            using (var domain = new StoryQXmlReportParserDomain(new StoryQXmlReportParserDomainConfig
                                                                        {
                                                                            TargetHealthCheckName = "StoryQTest",
                                                                            ReportFileTemplate = @"TestData\storyq.xml",
                                                                            BuildId = "12345"

                                                                        }))
            {
                Feature.WithScenario("A valid unzipped storyq xml report file is available")
                    .Given(domain.TheParserComponent)
                    .When(domain.TheParserIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 2)
                    .ExecuteWithReport();
            }
        }        

        [Test]
        public void NotZippedReportInBuildLocation()
        {
            using (var domain = new StoryQXmlReportParserDomain(new StoryQXmlReportParserDomainConfig
                                                                        {
                                                                            TargetHealthCheckName = "StoryQTest",
                                                                            ReportFileTemplate = @"TestData\12345\storyq.xml",
                                                                            BuildId = "12345"

                                                                        }))
            {
                Feature.WithScenario("A valid unzipped storyq xml report file is available in the build location")
                    .Given(domain.TheParserComponent)
                    .When(domain.TheParserIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 2)
                    .ExecuteWithReport();
            }
        }        

        [Test]
        public void ZippedFlatReportDefaultReportFilename()
        {
            using (var domain = new StoryQXmlReportParserDomain(new StoryQXmlReportParserDomainConfig
                                                                        {
                                                                            TargetHealthCheckName = "StoryQTest",
                                                                            ZipFileTemplate = @"TestData\storyq.zip",
                                                                            BuildId = "12345"
                                                                        }))
            {
                Feature.WithScenario("A valid zipped storyq xml report file is available in the root of the zip")
                    .Given(domain.TheParserComponent)
                    .When(domain.TheParserIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 2)
                    .ExecuteWithReport();
            }
        }        

        [Test]
        public void ZippedSubFolderReport()
        {
            using (var domain = new StoryQXmlReportParserDomain(new StoryQXmlReportParserDomainConfig
                                                                        {
                                                                            TargetHealthCheckName = "StoryQTest",
                                                                            ZipFileTemplate = @"TestData\storyq_subfolder.zip",
                                                                            ReportFileTemplate = @"StoryQ_Report\StoryQ.xml",
                                                                            BuildId = "12345"
                                                                        }))
            {
                Feature.WithScenario("A valid zipped storyq xml report file is available in a sub folder of the archive")
                    .Given(domain.TheParserComponent)
                    .When(domain.TheParserIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 2)
                    .ExecuteWithReport();
            }
        }
 
        [Test]
        public void FailedBuildWithTriggerResultNotSet()
        {
            using (var domain = new StoryQXmlReportParserDomain(new StoryQXmlReportParserDomainConfig
            {
                TargetHealthCheckName = "StoryQTest",
                ReportFileTemplate = @"TestData\storyq.xml",
                BuildId = "12345"
            }))
            {
                Feature.WithScenario("A valid unzipped storyq xml report file is available and the trigger result has not been set")
                    .Given(domain.TheParserComponent)
                    .When(domain.TheParserIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 2)
                    .ExecuteWithReport();
            }
        }        

        [Test]
        public void FailedBuildWithParserTriggerSetToFalse()
        {
            using (var domain = new StoryQXmlReportParserDomain(new StoryQXmlReportParserDomainConfig
            {
                TargetHealthCheckName = "StoryQTest",
                ReportFileTemplate = @"TestData\storyq.xml",
                BuildId = "12345",
                BuildResult = false,
                ParserMatchesToResult = false
            }))
            {
                Feature.WithScenario("A valid unzipped storyq xml report file is available and the parser trigger result is set to false")
                    .Given(domain.TheParserComponent)
                    .When(domain.TheParserIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 2)
                    .ExecuteWithReport();
            }
        }

        [Test]
        public void FailedBuildWithParserTriggerSetToTrue()
        {
            using (var domain = new StoryQXmlReportParserDomain(new StoryQXmlReportParserDomainConfig
            {
                TargetHealthCheckName = "StoryQTest",
                ReportFileTemplate = @"TestData\storyq.xml",
                BuildId = "12345",
                BuildResult = false,
                ParserMatchesToResult = true
            }))
            {
                Feature.WithScenario("A valid unzipped storyq xml report file is available and the parser trigger result is set to true")
                    .Given(domain.TheParserComponent)
                    .When(domain.TheParserIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 0)
                    .ExecuteWithReport();
            }
        }        
    }
}