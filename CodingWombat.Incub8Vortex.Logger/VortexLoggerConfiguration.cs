using Microsoft.Extensions.Logging;

namespace CodingWombat.Incub8Vortex.Logger
{
    public class VortexLoggerConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
        public int EventId { get; set; } = 0;
        public string ApiKey { get; set; }
    }
}