using CodingWombat.Incub8Vortex.Client.Client;
using CodingWombat.Incub8Vortex.Client.Configuration;
using CodingWombat.Incub8Vortex.Logger.LogEventDtos;

namespace CodingWombat.Incub8Vortex.Logger
{
    public class VortexLoggingClient : NonLoggingVortexClient<LogEventDto>
    {
        public VortexLoggingClient(VortexLoggerConfiguration configuration) : base(CreateClientConfig(configuration))
        {
        }

        static VortexConfiguration CreateClientConfig(VortexLoggerConfiguration configuration)
        {
            return new VortexConfiguration{ApiKey = configuration.ApiKey};
        }
    }
}