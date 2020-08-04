using System;
using System.Net.Http;
using System.Threading.Tasks;
using CodingWombat.Incub8Vortex.Client.Abstractions.Client;
using CodingWombat.Incub8Vortex.Client.Configuration;
using System.Net.Http.Json;
using System.Web;
using CodingWombat.Incub8Vortex.Client.Abstractions.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodingWombat.Incub8Vortex.Client.Client
{
    public class VortexClient<TEventDto> : IVortexClient<TEventDto> where TEventDto : IEventDto
    {
        private static readonly Uri BaseUrl = new Uri("https://vortex.incub8.de/api/event");
        private readonly ILogger<VortexClient<TEventDto>> _logger;
        private readonly VortexConfiguration _configuration;

        public VortexClient(ILogger<VortexClient<TEventDto>> logger, IOptions<VortexConfiguration> configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration.Value));
        }

        public async Task SendEventAsync(TEventDto eventDto)
        {

            var builder = new UriBuilder(BaseUrl);
            builder.Port = -1;
            
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["apikey"] = _configuration.ApiKey;
            builder.Query = query.ToString() ?? "";
            
            var client = new HttpClient {BaseAddress = builder.Uri};
            
            var postRequest = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
            {
                Content = JsonContent.Create(eventDto)
            };
            
            
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