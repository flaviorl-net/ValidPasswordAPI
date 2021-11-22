using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ValidPassword.Infra.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        //private readonly IDisposable _onChangeToken;
        private CustomLoggerProviderConfiguration _configuration;
        private readonly ConcurrentDictionary<string, Logger> _logger = new ConcurrentDictionary<string, Logger>();

        public CustomLoggerProvider(CustomLoggerProviderConfiguration configuration)
        {
            _configuration = configuration;
            //_onChangeToken = configuration.OnChange(updatedConfig => _configuration = updatedConfig);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _logger.GetOrAdd(categoryName, name => new Logger(name, _configuration));
        }

        public void Dispose()
        {
            _logger.Clear();
            //_onChangeToken.Dispose();
        }
    }
}
