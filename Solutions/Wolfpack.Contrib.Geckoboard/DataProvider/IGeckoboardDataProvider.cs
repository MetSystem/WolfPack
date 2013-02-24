using System.Collections.Generic;
using Wolfpack.Contrib.Geckoboard.Entities;
using Wolfpack.Core.Interfaces;

namespace Wolfpack.Contrib.Geckoboard.DataProvider
{
    /// <summary>
    /// This defines the methods to access the data required
    /// </summary>
    public interface IGeckoboardDataProvider : IDataProviderPlugin
    {
        IEnumerable<MapData> GetMapDataForCheck(MapArgs args);
        IEnumerable<PieChartData> GetPieChartDataForAllSites();
        IEnumerable<PieChartData> GetPieChartDataForSite(string site);
        IEnumerable<PieChartData> GetGeckoboardPieChartForCheck(PieChartArgs args);
        IEnumerable<LineChartData> GetLineChartDataForCheck(LineChartArgs args);
        IEnumerable<LineChartData> GetLineChartDataForCheckRate(LineChartArgs args);
        GeckometerData GetGeckoMeterDataForCheckAverage(GeckometerArgs args);
        GeckometerData GetGeckoMeterDataForCheck(GeckometerArgs args);
        ComparisonData GetComparisonDataForSiteCheck(ComparisonArgs args);
    }
}