using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace CloudMnD2._0.Core.ApplicationInsights
{
    public class T3TemplateTelemetryInitializer : ITelemetryInitializer
    {
        private readonly string RoleName;

        public T3TemplateTelemetryInitializer(string roleName)
        {
            RoleName = roleName;
        }

        public void Initialize(ITelemetry telemetry)
        {
            //If uncomment, Azure runtime will set this variable and code under if will not be called.
            //if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
            telemetry.Context.Cloud.RoleName = RoleName;
            telemetry.Context.Cloud.RoleInstance = "TestInstanceHost";
        }
    }
}
