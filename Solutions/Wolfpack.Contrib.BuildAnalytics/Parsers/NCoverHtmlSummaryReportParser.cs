using System;
using HtmlAgilityPack;
using System.Linq;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Publishers;

namespace Wolfpack.Contrib.BuildAnalytics.Parsers
{
    public class NCoverHtmlSummaryReportParserConfig : PluginConfigBase
    {
        public string TargetHealthCheckName { get; set; }
        public string ReportFile { get; set; }
    }

    public class NCoverHtmlSummaryReportParser : FilteredResultPublisherBase<NCoverHtmlSummaryReportParserConfig>
    {
        public NCoverHtmlSummaryReportParser(NCoverHtmlSummaryReportParserConfig config)
            : base(config, config.TargetHealthCheckName)
        {
        }

        protected override void Publish(HealthCheckResult buildResult)
        {
            if (string.IsNullOrWhiteSpace(Config.ReportFile))
                return;

            var report = new HtmlDocument();
            report.Load(Config.ReportFile);
            var nodes = report.DocumentNode.SelectNodes("//div[@id='page_stats']/div//span");

            if (nodes != null)
                nodes.Take(3).ToList().ForEach(n => Console.WriteLine(n.InnerText));
        }
    }
}