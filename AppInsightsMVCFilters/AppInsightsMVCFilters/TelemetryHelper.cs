﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Newtonsoft.Json;

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

        public void TrackRequest(string httpStatusCode, string methodName, bool isCallSuccess)
        {
            _stopwatch.Stop();
            TelemetryClient.TrackRequest(methodName, DateTimeOffset.UtcNow, _stopwatch.Elapsed, httpStatusCode, isCallSuccess);
        }

        public void LogPayload(HttpContent content, string eventName,Dictionary<string,string> customAttributes=null)
        {
            if (content != null)
            {
                var objectContent = content as ObjectContent;
                var payload = objectContent?.Value;
                var objectType = objectContent?.ObjectType;
                var payloadJson =JsonConvert.SerializeObject(payload);
                customAttributes = customAttributes ?? new Dictionary<string, string>();
                customAttributes.Add("type", objectType?.ToString());
                customAttributes.Add(eventName, payloadJson);
                TelemetryClient.TrackEvent(eventName,customAttributes);
            }
        }

        public void TrackException(Exception ex, string methodName)
        {
            TelemetryClient.TrackException(ex, new Dictionary<string, string>() { { "MethodName", methodName }, { "StackTrace", ex.StackTrace }, { "InnerException", ex.InnerException?.ToString() } });
        }

        public void TrackRequestUri(string eventName,string url, Dictionary<string,string> customAttributes = null )
        {
            customAttributes = customAttributes ?? new Dictionary<string, string>();
            customAttributes.Add("url", url);
            TelemetryClient.TrackEvent(eventName,customAttributes);
        }

    }
}
