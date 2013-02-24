using Wolfpack.Contrib.Geckoboard.DataProvider;
using Wolfpack.Contrib.Geckoboard.Entities;

namespace Wolfpack.Contrib.Geckoboard
{
    public interface IGeckoboardDataServiceImpl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        GeckoMap GetGeckoboardMapForCheck(MapArgs args);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        GeckoPieChart GetGeckoboardPieChartForAllSites();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        GeckoPieChart GetGeckoboardPieChartForSite(string site);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        GeckoPieChart GetGeckoboardPieChartForCheck(PieChartArgs args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        GeckoLineChart GetGeckoboardLineChartForCheck(LineChartArgs args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        GeckoLineChart GetGeckoboardLineChartForCheckRate(LineChartArgs args);

        /// <summary>
        /// This will get the min, max and average resultcount for a check
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        GeckoMeter GetGeckoboardGeckoMeterForCheckAverage(GeckometerArgs args);

        /// <summary>
        /// This will get the min, max and current (last) resultcount for a check
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        GeckoMeter GetGeckoboardGeckoMeterForCheck(GeckometerArgs args);

        /// <summary>
        /// This will get the last and previous to last resultcount for a specific site and check
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        GeckoComparison GetGeckoboardComparisonForSiteCheck(ComparisonArgs args);

        /// <summary>
        /// This will get the number of days interval from or to a date supplied
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        GeckoComparison GetGeckoboardDayInterval(string date);
    }
}