using System;
using System.Collections.Generic;
using CodingWombat.Incub8Vortex.Client.Abstractions.DTO;

namespace CodingWombat.Incub8Vortex.Client.Tests
{
    public class TestEventDto : IEventDto
    {
        public string Name { get; }
        public DateTime Timestamp { get; }
        public Dictionary<string, object> Attributes { get; set; }


        public TestEventDto(string name)
        {
            Name = name;
            Timestamp = DateTime.UtcNow;
        }
    }
}