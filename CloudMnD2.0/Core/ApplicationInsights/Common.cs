using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace CloudMnD2._0.Core.ApplicationInsights
{
    public static class Common
    {
        private static readonly string? ApplicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        public static string GetRoleName(string name)
        {
            return name;
        }

        public static TelemetryClient GetTelemetryClient(string roleName, string instrumentationKey)
        {
            roleName = GetRoleName(roleName);
            var configuration = TelemetryConfiguration.CreateDefault();
            configuration.InstrumentationKey = instrumentationKey;

            configuration.TelemetryInitializers.Add(new T3TemplateTelemetryInitializer(roleName));

            return new TelemetryClient(configuration);
        }
    }
}
