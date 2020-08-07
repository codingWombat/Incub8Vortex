using System;
using System.Reflection;
using CodingWombat.Incub8Vortex.Client.Client;
using CodingWombat.Incub8Vortex.Client.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace CodingWombat.Incub8Vortex.Client.Tests
{
    public class VortexClientTest
    {
        [Fact]
        public void TestBuildUri()
        {
            var logger = new Mock<ILogger<VortexClient<TestEventDto>>>();
            var options = new Mock<IOptions<VortexConfiguration>>();
            options.Setup(config => config.Value)
                .Returns(new VortexConfiguration {ApiKey = "1337"});
            
            var vortexClient = new VortexClient<TestEventDto>(logger.Object, options.Object);

            var methodInfo =
                typeof(VortexClient<TestEventDto>).GetMethod("BuildUri",
                    BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = {};
            var result = (Uri)methodInfo?.Invoke(vortexClient, parameters);
            Assert.Equal(result, new Uri("https://vortex.incub8.de/api/event?apikey=1337"));
        }
    }
}