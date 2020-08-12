using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace CodingWombat.Incub8Vortex.Logger
{
    internal class VortexLoggerConfigurationSetup : ConfigureFromConfigurationOptions<VortexLoggerConfiguration>
    {
        public VortexLoggerConfigurationSetup(ILoggerProviderConfiguration<VortexLoggerProvider> 
            providerConfiguration) : base(providerConfiguration.Configuration)
        {
        }
    }
}