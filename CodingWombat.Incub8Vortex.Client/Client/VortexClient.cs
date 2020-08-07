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

        public async Task SendBulkEventAsync(TEventDto[] eventDtos)
        {
            var uri = BuildUri();

            var postRequest = HttpRequestMessage(eventDtos, uri);
            

            await SendEventAsyncInternal(postRequest);
        }

        public async Task SendEventAsync(TEventDto eventDto)
        {
            var uri = BuildUri();

            var postRequest = HttpRequestMessage(eventDto, uri);

            await SendEventAsyncInternal(postRequest);
        }

        private HttpRequestMessage HttpRequestMessage(object payload, Uri uri)
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = JsonContent.Create(payload)
            };
            return postRequest;
        }

        private async Task SendEventAsyncInternal(HttpRequestMessage postRequest)
        {
            var client = new HttpClient();
            
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

        
        private Uri BuildUri()
        {
            var builder = new UriBuilder(BaseUrl) {Port = -1};

            var query = HttpUtility.ParseQueryString(builder.Query);
            query["apikey"] = _configuration.ApiKey;
            builder.Query = query.ToString() ?? "";
            return builder.Uri;
        }
    }
}