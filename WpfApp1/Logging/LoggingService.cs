using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.EventSourceListener;
using Microsoft.ApplicationInsights.Extensibility;

namespace WpfApp1.Logging
{
    public class LoggingService
    {
        public const string ApplicationConfigurationEventSourceName = "Company-Product-Application";

        public static void EnableLogging()
        {
            var configuration = TelemetryConfiguration.Active;

            // subscribe to event sources
            var eventLoggingModule = new EventSourceTelemetryModule();
            eventLoggingModule.Sources.Add(new EventSourceListeningRequest { Name = ApplicationConfigurationEventSourceName, Level = System.Diagnostics.Tracing.EventLevel.Informational });

            eventLoggingModule.Initialize(configuration);

            // enable dependency tracking
            var module = new DependencyTrackingTelemetryModule();

            // prevent Correlation Id to be sent to certain endpoints. You may add other domains as needed.
            module.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("core.windows.net");
            //...

            // enable known dependency tracking, note that in future versions, we will extend this list.
            // please check default settings in https://github.com/Microsoft/ApplicationInsights-dotnet-server/blob/develop/Src/DependencyCollector/NuGet/ApplicationInsights.config.install.xdt#L20
            module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
            module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.EventHubs");
            //....

            // initialize the module
            module.Initialize(configuration);
        }

        public static void DisableLogging()
        {
            TelemetryConfiguration.Active.InstrumentationKey = string.Empty;
            TelemetryConfiguration.Active.DisableTelemetry = true;
        }
    }
}