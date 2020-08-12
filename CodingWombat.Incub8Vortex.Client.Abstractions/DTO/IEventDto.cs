using System;
using System.Collections.Generic;

namespace CodingWombat.Incub8Vortex.Client.Abstractions.DTO
{
    public interface IEventDto
    {
        public string Name { get; }
        public DateTime Timestamp { get; }
        public Dictionary<string, object> Attributes { get; set; }
    }
}