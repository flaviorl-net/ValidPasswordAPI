using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace ValidPassword.Infra.Logging
{
    public class Logger : ILogger
    {
        private string _LoggerName { get; set; }

        private CustomLoggerProviderConfiguration _Configuration { get; set; }
        
        public Logger(string name, CustomLoggerProviderConfiguration configuration)
        {
            _LoggerName = name;
            _Configuration = configuration;
        }

        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel) => _Configuration.LogLevel == logLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = string.Format("{0} - {1} - {2}", logLevel, eventId, formatter(state, exception));
            GravarLog(message);
        }

        public void GravarLog(string message)
        {
            using (StreamWriter file = new StreamWriter("Logs.txt", append: true))
            {
                file.WriteLine(message);
            }
        }

    }
}
