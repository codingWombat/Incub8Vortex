using System;
using System.Reflection;
using CodingWombat.Incub8Vortex.Client.Client;
using CodingWombat.Incub8Vortex.Client.Configuration;
using Xunit;

namespace CodingWombat.Incub8Vortex.Client.Tests
{
    public class VortexClientTest
    {
        [Fact]
        public void TestBuildUri()
        {
            
            var vortexClient = new NonLoggingVortexClient<TestEventDto>(new VortexConfiguration {ApiKey = "1337"});

            var methodInfo =
                typeof(NonLoggingVortexClient<TestEventDto>).GetMethod("BuildUri",
                    BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = {};
            var result = (Uri)methodInfo?.Invoke(vortexClient, parameters);
            Assert.Equal(new Uri("https://vortex.incub8.de/api/event?apikey=1337"), result );
        }
    }
}