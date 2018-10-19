using System;
using System.Diagnostics.Tracing;

namespace WpfApp1.Logging
{
    [EventSource(Name = LoggingService.ApplicationConfigurationEventSourceName)]
    internal sealed class ApplicationEventSource : EventSource
    {
        private ApplicationEventSource()
        {
        }

        private static readonly Lazy<ApplicationEventSource> instance = new Lazy<ApplicationEventSource>(() => new ApplicationEventSource());

        public static ApplicationEventSource Log => instance.Value;

        [Event(1, Level = EventLevel.Informational)]
        public void LogStart(string message)
        {
            if (this.IsEnabled(EventLevel.Informational, EventKeywords.All))
            {
                this.WriteEvent(1, message);
            }
        }
    }
}