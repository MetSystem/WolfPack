
using System;
using System.Collections.Generic;
using Wolfpack.Core.Interfaces.Entities;
using System.Linq;

namespace Wolfpack.Core.Filters.Notification
{
    public class StateChangeNagFailNotificationFilter : StateChangeNotificationFilter
    {
        private List<KeyValuePair<int, int>> _alertSpacing;

        public StateChangeNagFailNotificationFilter()
            : this(new KeyValuePair<int, int>(0, 1),
            new KeyValuePair<int, int>(3, 3),
            new KeyValuePair<int, int>(5, 10),
            new KeyValuePair<int, int>(10, 60))
        {
        }

        public StateChangeNagFailNotificationFilter(params KeyValuePair<int, int>[] alertSpacings)
        {
            _alertSpacing = new List<KeyValuePair<int, int>>(alertSpacings);
        }

        public override string Mode
        {
            get { return "StateChangeNagFail"; }
        }

        protected override bool HasStateChanged(HealthCheckResult result, AlertHistory alertHistory)
        {
            var changed = alertHistory.LastResult != result.Check.Result;

            if (changed)
            {
                if (alertHistory.LastResult.GetValueOrDefault(false))
                {
                    // flipped to success from failure
                    alertHistory.FailuresSinceLastSuccess = 0;
                }

                return true;
            }
           
            //  not changed - do we need to consider frequency of our nagging?
            if (alertHistory.LastResult.HasValue)
            {
                if (alertHistory.LastResult.Value)
                {
                    // stream of success messages
                    return false;
                }

                // stream of failures...check frequency                
                alertHistory.FailuresSinceLastSuccess++;
                var minimumMinutesSeparation = FindAlertSeparation(alertHistory.FailuresSinceLastSuccess);                
                var minutesDiff = DateTime.UtcNow.Subtract(alertHistory.LastReceived).TotalMinutes;               
                return (minutesDiff >= minimumMinutesSeparation);
            }
            
            return true;
        }

        private int FindAlertSeparation(int failuresSinceLastSuccess)
        {
            return _alertSpacing.Where(spacing => spacing.Key <= failuresSinceLastSuccess)
                .OrderByDescending(spacing => spacing.Key)
                .First().Value;
        }
    }
}