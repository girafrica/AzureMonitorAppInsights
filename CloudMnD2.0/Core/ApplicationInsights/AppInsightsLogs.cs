using CloudMnD2._0.Core.Exception;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;

namespace CloudMnD2._0.Core.ApplicationInsights
{
    public class AppInsightsLogs
    {
        private static TelemetryClient TelemetryClient;
        private static AppInsightsLogs instance;
        public static AppInsightsLogs Instance => instance ?? throw new
            NullReferenceException("The TelemetryClient instance has not been initialized. You must call AppInsightsLogs.Initialize before calling AppInsightsLogs.Instance");

        private AppInsightsLogs(string roleName)
        {
            TelemetryClient = Common.GetTelemetryClient(roleName, ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"]);
        }

        public static void Initialize(string roleName)
        {
            instance = new AppInsightsLogs(roleName);
        }

        public void GenerateRequest(bool isSuccess)
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            var randomDuration = rnd.Next(0, 30);
            var requestTelemetry = new RequestTelemetry {Duration = new TimeSpan(0, 0, 0, randomDuration)};
            if (isSuccess)
            {
                requestTelemetry.Properties["isCached"] = "false";
                requestTelemetry.Name = "SuccessRequest";
                requestTelemetry.ResponseCode = "200";
                requestTelemetry.Success = true;
            }
            if (!isSuccess)
            {
                requestTelemetry.Name = "FailedRequest";
                requestTelemetry.ResponseCode = "500";
                requestTelemetry.Success = false;
            }
            TelemetryClient.TrackRequest(requestTelemetry);
        }

        public void GenerateException(string operationId = "")
        {
            var exceptionTelemetry = new ExceptionTelemetry
            {
                Exception = new FakeExceptionByMnD(),
                Message = "It's a test exception. Don't worry :)",
                SeverityLevel = SeverityLevel.Error
            };

            exceptionTelemetry.Context.Operation.Name = "Some operation name";
            exceptionTelemetry.Context.Operation.Id = operationId;

            TelemetryClient.TrackException(exceptionTelemetry);
        }

        public void Flush()
        {
            TelemetryClient.Flush();
        }

        public void GenerateAvailability(bool success = true)
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            var randomDuration = rnd.Next(0, 30);
            var startDate = DateTime.UtcNow;
            var availabilityTelemetry = new AvailabilityTelemetry("SomeApp",
                startDate, new TimeSpan(0, 0, 0, randomDuration), "Some location", success);

            TelemetryClient.TrackAvailability(availabilityTelemetry);
        }
    }
}
