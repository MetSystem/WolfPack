using System;
using HtmlAgilityPack;
using System.Linq;

namespace Wolfpack.Contrib.BuildParser.Parsers
{
    public class NCoverHtmlSummaryReportParser
    {
        public void Execute(BuildParserCheckConfig config)
        {
            if (string.IsNullOrWhiteSpace(config.NCoverHtmlSummaryResultsFile))
                return;

            var report = new HtmlDocument();
            report.Load(config.NCoverHtmlSummaryResultsFile);
            var nodes = report.DocumentNode.SelectNodes("//div[@id='page_stats']/div//span");

            if (nodes != null)
                nodes.Take(3).ToList().ForEach(n => Console.WriteLine(n.InnerText));
        }
    }
}