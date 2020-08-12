using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace CodingWombat.Incub8Vortex.Logger
{
    [ProviderAlias("Vortex")]
    public class VortexLoggerProvider : ILoggerProvider
    {
        private readonly VortexLoggerConfiguration _configuration;

        private readonly ConcurrentDictionary<string, VortexLogger> _loggers =
            new ConcurrentDictionary<string, VortexLogger>();

        public VortexLoggerProvider(VortexLoggerConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Dispose()
        {
            _loggers.Clear();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new VortexLogger(name, _configuration));
        }
    }
}