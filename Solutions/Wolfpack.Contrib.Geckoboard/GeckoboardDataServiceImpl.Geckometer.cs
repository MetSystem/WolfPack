using System;
using Wolfpack.Contrib.Geckoboard.DataProvider;
using Wolfpack.Contrib.Geckoboard.Entities;
using Wolfpack.Core;

namespace Wolfpack.Contrib.Geckoboard
{
    public partial class GeckoboardDataServiceImpl
    {
        /// <summary>
        /// This will get the min, max and average resultcount for a specific site and check
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual GeckoMeter GetGeckoboardGeckoMeterForCheckAverage(GeckometerArgs args)
        {
            var data = new GeckoMeter
            {
                DecimalPlaces = args.DecimalPlaces
            };

            try
            {
                var rawData = myDataProvider.GetGeckoMeterDataForCheckAverage(args);

                data.Item = rawData.Value;
                data.Min.Text = "Min";
                data.Min.Value = rawData.Min;
                data.Max.Text = "Max";
                data.Max.Value = rawData.Max;
            }
            catch (Exception ex)
            {
                Logger.Error(Logger.Event.During("GetGeckoboardGeckoMeterForCheckAverage")
                    .Encountered(ex));
                throw;
            }

            return data;
        }               

        /// <summary>
        /// This will get the min, max and current (last) resultcount for a check
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual GeckoMeter GetGeckoboardGeckoMeterForCheck(GeckometerArgs args)
        {
            var data = new GeckoMeter
            {
                DecimalPlaces = args.DecimalPlaces
            };

            try
            {
                var rawData = myDataProvider.GetGeckoMeterDataForCheck(args);

                data.Item = rawData.Value;
                data.Min.Text = "Min";
                data.Min.Value = rawData.Min;
                data.Max.Text = "Max";
                data.Max.Value = rawData.Max;
            }
            catch (Exception ex)
            {
                Logger.Error(Logger.Event.During("GetGeckoboardGeckoMeterForCheck")
                    .Encountered(ex));
                throw;
            }

            return data;
        }               
    }
}