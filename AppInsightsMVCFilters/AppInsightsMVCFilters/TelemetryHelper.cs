using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace AppInsightsMVCFilters
{
    public class TelemetryHelper
    {
        public TelemetryClient TelemetryClient { get; private set; }
        private static readonly TelemetryClient TelemetryClientSingleton = new TelemetryClient();
        RequestTelemetry _telemetry = null;
        private Stopwatch _stopwatch = null;
        public TelemetryHelper()
        {
            TelemetryClient = TelemetryClientSingleton;
        }

        public void Start(string methodName)
        {
            _telemetry = new RequestTelemetry();
            _telemetry.Context.Operation.Id = Guid.NewGuid().ToString();
            _telemetry.Context.Operation.Name = methodName;
            _stopwatch = Stopwatch.StartNew();
        }

        public void TrackRequest()
        {
            TelemetryClient.TrackRequest(_telemetry);
        }

        public void TrackRequest(string httpStatusCode, string methodName, bool isCallSuccess)
        {
            _stopwatch.Stop();
            TelemetryClient.TrackRequest(methodName, DateTimeOffset.UtcNow, _stopwatch.Elapsed, httpStatusCode, isCallSuccess);
        }

        public void TrackException(Exception ex, string methodName)
        {
            TelemetryClient.TrackException(ex, new Dictionary<string, string>() { { "MethodName", methodName }, { "StackTrace", ex.StackTrace }, { "InnerException", ex.InnerException?.ToString() } });
        }

    }
}
