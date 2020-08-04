using System;
using System.Net.Http;
using System.Threading.Tasks;
using CodingWombat.Incub8Vortex.Client.Abstractions.Client;
using CodingWombat.Incub8Vortex.Client.Configuration;
using CodingWombat.Incub8Vortex.Client.DTO;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodingWombat.Incub8Vortex.Client.Client
{
    public class VortexClient : IVortexClient<EventDto>
    {
        private static readonly Uri BaseUrl = new Uri("https://vortex.incub8.de/api/event");
        private readonly ILogger<VortexClient> _logger;
        private readonly VortexConfiguration _configuration;

        public VortexClient(ILogger<VortexClient> logger, IOptions<VortexConfiguration> configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration.Value));
        }

        public async Task SendEventAsync(EventDto eventDto)
        {
            var client = new HttpClient {BaseAddress = BaseUrl};
            
            var postRequest = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
            {
                Content = JsonContent.Create(eventDto)
            };

            postRequest.Properties.Add("apikey", _configuration.ApiKey);
            
            try
            {
                var response = await client.SendAsync(postRequest);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                
                if (!content.Equals("[]"))
                {
                    _logger.LogError("Response body not empty: {}", content);    
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("Event sending failed!", e);
            }
        }
    }
}