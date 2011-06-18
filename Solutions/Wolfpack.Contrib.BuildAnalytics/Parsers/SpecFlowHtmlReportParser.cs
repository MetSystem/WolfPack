using HtmlAgilityPack;
using Wolfpack.Contrib.BuildAnalytics.Interfaces.Entities;
using Wolfpack.Contrib.BuildAnalytics.Publishers;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Contrib.BuildAnalytics.Parsers
{
    public class SpecFlowHtmlReportParserConfig : BuildParserConfigBase
    {
    }

    public class SpecFlowHtmlReportParser : BuildResultPublisherBase<SpecFlowHtmlReportParserConfig>
    {
        public const string SpecFlowReport = "SpecFlow";

        public SpecFlowHtmlReportParser(SpecFlowHtmlReportParserConfig config)
            : base(config, config.TargetHealthCheckName)
        {
        }

        protected override void Publish(HealthCheckResult buildResult)
        {
            if (string.IsNullOrWhiteSpace(Config.ZipFileTemplate) &&
                string.IsNullOrWhiteSpace(Config.ReportFileTemplate))
                return;

            string reportContent;
            if (!GetContent(buildResult, "AcceptanceTestReport.htm", out reportContent))
                return;

            // finally parse the content for the counts
            /*
1=0 features
            
2=90%
3=&nbsp;&nbsp;
4=168
5=151
6=0
7=0
8=17            
             */
            var report = new HtmlDocument();
            report.LoadHtml(reportContent);
            var nodes = report.DocumentNode.SelectNodes("html/body/table[@class='reportTable']/tr[2]/td");

            PublishStat(buildResult, SpecFlowReport, ExtractStat(nodes[1].InnerText), "SuccessRate");
            PublishStat(buildResult, SpecFlowReport, ExtractStat(nodes[3].InnerText), "Total");
            PublishStat(buildResult, SpecFlowReport, ExtractStat(nodes[4].InnerText), "Success");
            PublishStat(buildResult, SpecFlowReport, ExtractStat(nodes[5].InnerText), "Failures");
            PublishStat(buildResult, SpecFlowReport, ExtractStat(nodes[6].InnerText), "Pending");
            PublishStat(buildResult, SpecFlowReport, ExtractStat(nodes[7].InnerText), "Ignored");
        }
    }
}