using System;
using System.Collections.Generic;
using Nancy;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.WebServices;
using Wolfpack.Core.WebUI.Interfaces.Entities;

namespace Wolfpack.Core.WebUI.Modules
{
    public class UiViewsModule : NancyModule
    {
        private const string BaseUrl = "/ui";

        private readonly ActivityTracker _tracker;

        public UiViewsModule(ActivityTracker tracker) 
            : base(BaseUrl)
        {
            _tracker = tracker;
            Get["/"] = _ => View["views/status.sshtml"];
            Get["/status"] = _ => View["views/status.sshtml"];
            Get["/activity"] = _ => View["views/activity.sshtml"];
            Get["/gallery"] = _ => View["views/gallery.sshtml"];
            Get["/configure"] = _ => View["views/configure.sshtml"];
            Get["/about"] = _ => View["views/about.sshtml"];
            Get["/tools/diagnostics"] = _ => View["views/diagnostics.sshtml"];
            Get["/tools/sendnotification"] = _ =>
            {
                var siteInfo = _tracker.StartEvent;

                var notification = new NotificationEvent
                {
                    AgentId = siteInfo.AgentId,
                    CheckId = "CheckId",
                    CriticalFailure = false,
                    CriticalFailureDetails = null,
                    Data = "JSON Serialised copy of this...",
                    MinuteBucket = AnalyticsBaseline.MinuteBucket,
                    HourBucket = AnalyticsBaseline.HourBucket,
                    DayBucket = AnalyticsBaseline.DayBucket,
                    DisplayUnit = "",
                    EventType = "",
                    GeneratedOnUtc = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    Latitude = "",
                    Longitude = "",
                    Message = "Message",
                    Properties = new Properties { {"SomeProperty", "SomeValue"} },
                    Result = false,
                    ResultCount = 0,
                    SiteId = _tracker.StartEvent.SiteId,
                    State = MessageStateTypes.NotSet,
                    Tags = new List<string> {"SomeTag"},
                    Version = Guid.Empty
                };
                var model = new SendNotificationModel(notification);
                return View["views/sendnotification.sshtml", model];
            };
        }
    }
}