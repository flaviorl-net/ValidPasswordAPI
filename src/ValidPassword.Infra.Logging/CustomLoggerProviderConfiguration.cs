using Microsoft.Extensions.Logging;
using System;

namespace ValidPassword.Infra.Logging
{
    public class CustomLoggerProviderConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        public int EventId { get; set; }
    }
}