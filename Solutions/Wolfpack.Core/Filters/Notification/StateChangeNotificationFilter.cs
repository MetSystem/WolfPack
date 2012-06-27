using System;
using System.Collections.Generic;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Filters.Notification
{
    public class AlertHistory
    {
        public DateTime LastReceived { get; set; }
        public bool? LastResult { get; set; }
        public int FailuresSinceLastSuccess { get; set; }
    }

    public class StateChangeNotificationFilter : NotificationFilterBase
    {
        protected readonly Dictionary<string, AlertHistory> _history;

        public StateChangeNotificationFilter()
        {
            _history = new Dictionary<string, AlertHistory>();
        }

        public override string Mode
        {
            get { return "StateChange"; }
        }

        public override void Execute(HealthCheckResult result)
        {
            lock (_history)
            {
                var key = GetKey(result);
                AlertHistory alertHistory;

                if (!_history.ContainsKey(key))
                {
                    alertHistory = new AlertHistory
                    {
                        LastResult = result.Check.Result,
                        LastReceived = DateTime.UtcNow
                    };

                    if (!result.Check.Result.GetValueOrDefault(true))
                        alertHistory.FailuresSinceLastSuccess++;

                    HandleFirstAlert(result, alertHistory);
                    return;
                }

                // has state change from last?
                alertHistory = _history[key];

                if (!HasStateChanged(result, alertHistory))
                {
                    Logger.Debug("State for Check '{0}' is unchanged, not publishing", result.Check.Identity.Name);
                    return;
                }

                HandleStateChange(result, alertHistory);
            }
        }

        protected virtual void HandleFirstAlert(HealthCheckResult result, AlertHistory alertHistory)
        {
            _history.Add(GetKey(result), alertHistory);
            Messenger.Publish(result);
        }

        protected virtual bool HasStateChanged(HealthCheckResult result, AlertHistory alertHistory)
        {
            return alertHistory.LastResult != result.Check.Result;
        }

        protected virtual void HandleStateChange(HealthCheckResult result, AlertHistory alertHistory)
        {
            Messenger.Publish(result);
            alertHistory.LastResult = result.Check.Result;
            alertHistory.LastReceived = DateTime.UtcNow;
        }
    }
}