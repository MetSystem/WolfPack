using System;
using Wolfpack.Contrib.BuildParser;
using Wolfpack.Contrib.BuildParser.Parsers;
using Wolfpack.Tests.Bdd;

namespace Wolfpack.Tests.BuildParser
{
    public class NCoverSummaryReportParserDomain : BddTestDomain
    {
        protected NCoverHtmlSummaryReportParser myParser;


        public override void Dispose()
        {
            
        }

        public void TheParserComponent()
        {
            myParser = new NCoverHtmlSummaryReportParser();
        }

        public void TheParserIsInvoked()
        {
            myParser.Execute(new BuildParserCheckConfig
                                 {
                                     NCoverHtmlSummaryResultsFile = @"c:\temp\ncover\report\report.html"
                                 });
        }

        public void TheCoverageSummaryValuesShouldBeAvailable()
        {
            throw new NotImplementedException();
        }
    }
}