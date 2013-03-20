using System;
using System.Collections.Generic;
using System.Linq;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Notification.Filters.Request
{
    public class StateChangeNagFailNotificationFilter : StateChangeNotificationFilter
    {
        public new const string FilterName = "StateChangeNagFail";

        private readonly List<KeyValuePair<int, int>> _alertSpacing;

        public StateChangeNagFailNotificationFilter()
            : this(new KeyValuePair<int, int>(0, 1),
            new KeyValuePair<int, int>(3, 3),
            new KeyValuePair<int, int>(15, 10),
            new KeyValuePair<int, int>(35, 60))
        {
        }

        public StateChangeNagFailNotificationFilter(params KeyValuePair<int, int>[] alertSpacings)
        {
            _alertSpacing = new List<KeyValuePair<int, int>>(alertSpacings);
        }

        public override string Mode { get { return FilterName; } }

        protected override bool HasStateChanged(NotificationRequest request, AlertHistory alertHistory)
        {
            var changed = alertHistory.LastResult != request.Notification.Result;

            if (changed)
            {
                if (alertHistory.LastResult.GetValueOrDefault(false))
                {
                    // flipped to success from failure
                    alertHistory.FailuresSinceLastSuccess = 0;
                }

                Logger.Debug("{0} state changed!", request.CheckId);
                return true;
            }
           
            //  not changed - do we need to consider frequency of our nagging?
            if (alertHistory.LastResult.HasValue)
            {
                if (alertHistory.LastResult.Value)
                {
                    // stream of success messages
                    Logger.Debug("{0} result:={1} (success stream)", alertHistory.LastResult.Value);
                    return false;
                }

                // stream of failures...check frequency                
                alertHistory.FailuresSinceLastSuccess++;
                var minimumMinutesSeparation = FindAlertSeparation(alertHistory.FailuresSinceLastSuccess);                
                var minutesDiff = DateTime.UtcNow.Subtract(alertHistory.LastReceived).TotalMinutes;

                Logger.Debug("{0} failures;={1}, separation:={2}, lastdiff:={3}", request.CheckId, alertHistory.FailuresSinceLastSuccess, minimumMinutesSeparation, minutesDiff);
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