using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(IntegrationTests.Startup))]

namespace IntegrationTests
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            TelemetryConfiguration.Active.InstrumentationKey = "Sample";
           
            ConfigureAuth(app);
        }
    }
}
