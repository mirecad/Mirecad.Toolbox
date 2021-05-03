using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Mirecad.Toolbox.Extensions
{
    public static class TelemetryClientExtensions
    {
        public static void TrackTraceWithContent(this TelemetryClient client, string categoryName, string value)
        {
            client.TrackTrace(categoryName, SeverityLevel.Information, new Dictionary<string, string> { { "Content", value } });
        }

        public static void TrackEventWithEventKey(this TelemetryClient client, string eventKey, string value)
        {
            client.TrackEvent(value, new Dictionary<string, string> { { "EventKey", eventKey } });
        }
    }
}