using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Wolfpack.Contrib.BuildAnalytics.Interfaces.Entities;
using Wolfpack.Contrib.BuildAnalytics.Publishers;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Contrib.BuildAnalytics.Parsers
{
    public class StoryQXmlReportParserConfig : BuildParserConfigBase
    {
    }

    public class StoryQXmlReportParser : BuildResultPublisherBase<StoryQXmlReportParserConfig>
    {
        public const string StoryQReport = "StoryQ";

        public StoryQXmlReportParser(StoryQXmlReportParserConfig config)
            : base(config, config.TargetHealthCheckName)
        {
        }

        protected override void Publish(HealthCheckResult buildResult)
        {
            if (string.IsNullOrWhiteSpace(Config.ZipFileTemplate) &&
                string.IsNullOrWhiteSpace(Config.ReportFileTemplate))
                return;

            string reportContent;
            if (!GetContent(buildResult, "StoryQ.xml", out reportContent))
                return;

            // finally parse the content for the counts
            var xdoc = XDocument.Parse(reportContent);
            var resultSummary =
                (from results in xdoc.XPathSelectElements("/StoryQRun/Project/Namespace/Class/Method/Story/Result")
                 group results by results.Attribute("Type").Value
                 into resultGroups
                 select new {Result = resultGroups.Key, Count = resultGroups.Count()}).ToList();

            resultSummary.ForEach(rs => PublishStat(buildResult, StoryQReport, rs.Count, rs.Result));
        }
    }
}