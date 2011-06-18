using NUnit.Framework;
using StoryQ;
using Wolfpack.Tests.Bdd;

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
    }
}