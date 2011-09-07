using NUnit.Framework;
using StoryQ;
using Wolfpack.Core.Testing.Bdd;

namespace Wolfpack.Tests.BuildAnalytics
{
    [TestFixture]
    public class SpecFlowHtmlReportParserSpecs : BddFeature
    {
        protected override Feature DescribeFeature()
        {
            return new Story("Be able to consume the statistcs from a SpecFlow report")
                .InOrderTo("Visualise and monitor SpecFlow information")
                .AsA("Sys Admin")
                .IWant("To be able to provide a report and have it parsed into health check result messages");
        }

        [Test]
        public void UnzippedReport()
        {
            using (var domain = new SpecFlowHtmlReportParserDomain(new SpecFlowHtmlReportParserDomainConfig
                                                                        {
                                                                            TargetHealthCheckName = "SpecFlowTest",
                                                                            ReportFileTemplate = @"TestData\AcceptanceTestReport.htm",
                                                                            BuildId = "12345"

                                                                        }))
            {
                Feature.WithScenario("A valid unzipped SpecFlow xml report file is available")
                    .Given(domain.TheParserComponent)
                    .When(domain.TheParserIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 6)
                    .ExecuteWithReport();
            }
        }        

        [Test]
        public void ZippedFlatReportDefaultReportFilename()
        {
            using (var domain = new SpecFlowHtmlReportParserDomain(new SpecFlowHtmlReportParserDomainConfig
                                                                        {
                                                                            TargetHealthCheckName = "SpecFlowTest",
                                                                            ZipFileTemplate = @"TestData\SpecFlow.zip",
                                                                            BuildId = "12345"
                                                                        }))
            {
                Feature.WithScenario("A valid zipped SpecFlow xml report file is available in the root of the zip")
                    .Given(domain.TheParserComponent)
                    .When(domain.TheParserIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 6)
                    .ExecuteWithReport();
            }
        }        

        [Test]
        public void ZippedSubFolderReport()
        {
            using (var domain = new SpecFlowHtmlReportParserDomain(new SpecFlowHtmlReportParserDomainConfig
                                                                        {
                                                                            TargetHealthCheckName = "SpecFlowTest",
                                                                            ZipFileTemplate = @"TestData\SpecFlow_subfolder.zip",
                                                                            ReportFileTemplate = @"SpecFlow\AcceptanceTestReport.htm",
                                                                            BuildId = "12345"
                                                                        }))
            {
                Feature.WithScenario("A valid zipped SpecFlow xml report file is available in a sub folder of the archive")
                    .Given(domain.TheParserComponent)
                    .When(domain.TheParserIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 6)
                    .ExecuteWithReport();
            }
        }        
    }
}