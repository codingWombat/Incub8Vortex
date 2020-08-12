using System;
using System.Collections.Generic;
using CodingWombat.Incub8Vortex.Client.Abstractions.DTO;

namespace CodingWombat.Incub8Vortex.Logger.LogEventDtos
{
    public class LogEventDto : IEventDto
    {
        public string Name { get; } = "LogEvent";
        public DateTime Timestamp { get; } = DateTime.UtcNow;
        public Dictionary<string, object> Attributes { get; set; }
    }
}