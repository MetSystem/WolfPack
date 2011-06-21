
using HtmlAgilityPack;
using Wolfpack.Contrib.BuildAnalytics.Interfaces.Entities;
using Wolfpack.Contrib.BuildAnalytics.Publishers;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Contrib.BuildAnalytics.Parsers
{
    public class NCoverHtmlSummaryReportParserConfig : BuildParserConfigBase
    {
    }

    public class NCoverHtmlSummaryReportParser : BuildResultPublisherBase<NCoverHtmlSummaryReportParserConfig>
    {
        public const string NCoverReport = "NCover";

        public NCoverHtmlSummaryReportParser(NCoverHtmlSummaryReportParserConfig config)
            : base(config, config.TargetHealthCheckName)
        {
        }

        protected override void Publish(HealthCheckResult buildResult)
        {
            if (string.IsNullOrWhiteSpace(Config.ZipFileTemplate) &&
                string.IsNullOrWhiteSpace(Config.ReportFileTemplate))
                return;

            string reportContent;
            if (!GetContent(buildResult, "ncover-summary-report.html", out reportContent))
                return;

            var report = new HtmlDocument();
            report.LoadHtml(reportContent);
            var nodes = report.DocumentNode.SelectNodes("//div[@id='page_stats']/div//span");

            PublishStat(buildResult, NCoverReport, ExtractStat(nodes[0].InnerText), "symbol");
            PublishStat(buildResult, NCoverReport, ExtractStat(nodes[1].InnerText), "branch");
            PublishStat(buildResult, NCoverReport, ExtractStat(nodes[2].InnerText), "method");
            PublishStat(buildResult, NCoverReport, ExtractStat(nodes[3].InnerText), "cyclomatic-complexity-avg");
        }
    }
}