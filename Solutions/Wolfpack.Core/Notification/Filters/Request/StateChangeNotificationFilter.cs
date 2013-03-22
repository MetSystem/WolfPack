using System;
using System.Collections.Generic;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Notification.Filters.Request
{
    public class AlertHistory
    {
        public DateTime LastReceived { get; set; }
        public bool? LastResult { get; set; }
        public int FailuresSinceLastSuccess { get; set; }
    }

    public class StateChangeNotificationFilter : NotificationRequestFilterBase
    {
        public const string FilterName = "StateChange";

        protected readonly Dictionary<string, AlertHistory> _history;

        public StateChangeNotificationFilter()
        {
            _history = new Dictionary<string, AlertHistory>();
        }

        public override string Mode { get { return FilterName; } }

        public override bool Execute(NotificationRequest request)
        {
            lock (_history)
            {
                var key = GetKey(request);
                AlertHistory alertHistory;

                if (!_history.ContainsKey(key))
                {
                    alertHistory = new AlertHistory
                    {
                        LastResult = request.Notification.Result,
                        LastReceived = DateTime.UtcNow
                    };

                    if (!request.Notification.Result.GetValueOrDefault(true))
                        alertHistory.FailuresSinceLastSuccess++;

                    HandleFirstAlert(request, alertHistory);
                    return true;
                }

                // has state change from last?
                alertHistory = _history[key];

                if (!HasStateChanged(request, alertHistory))
                {
                    Logger.Debug("State for Check '{0}' is unchanged, not publishing", request.CheckId);
                    return false;
                }

                HandleStateChange(request, alertHistory);
                return true;
            }
        }

        protected virtual void HandleFirstAlert(NotificationRequest request, AlertHistory alertHistory)
        {
            _history.Add(GetKey(request), alertHistory);
        }

        protected virtual bool HasStateChanged(NotificationRequest request, AlertHistory alertHistory)
        {
            return alertHistory.LastResult != request.Notification.Result;
        }

        protected virtual void HandleStateChange(NotificationRequest request, AlertHistory alertHistory)
        {
            alertHistory.LastResult = request.Notification.Result;
            alertHistory.LastReceived = DateTime.UtcNow;
        }
    }
}
