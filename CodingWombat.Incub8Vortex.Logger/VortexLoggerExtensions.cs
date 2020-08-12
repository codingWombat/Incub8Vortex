using CodingWombat.Incub8Vortex.Client.Configuration;
using Microsoft.Extensions.Logging;

namespace CodingWombat.Incub8Vortex.Logger
{
    public static class VortexLoggerExtensions
    {
        public static ILoggerFactory  AddVortexLogger(this ILoggerFactory  loggerFactory, VortexLoggerConfiguration configuration)
        {
            loggerFactory.AddProvider(new VortexLoggerProvider(configuration));
            return loggerFactory;
        }
    }
}