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
            return new Story("Be able to stream data and optionally set a threshold that triggers an alert")
                .InOrderTo("Capture data and receive alerts")
                .AsA("user")
                .IWant("The component to behave correctly");
        }

        [Test]
        public void UnzippedReport()
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