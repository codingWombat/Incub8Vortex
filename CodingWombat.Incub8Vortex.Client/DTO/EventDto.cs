using System;
using System.Collections.Generic;
using CodingWombat.Incub8Vortex.Client.Abstractions.DTO;

namespace CodingWombat.Incub8Vortex.Client.DTO
{
    public class EventDto :IEventDto
    {
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }
}