using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using CodingWombat.Incub8Vortex.Client.Abstractions.Client;
using CodingWombat.Incub8Vortex.Client.Abstractions.DTO;
using CodingWombat.Incub8Vortex.Client.Configuration;

namespace CodingWombat.Incub8Vortex.Client.Client
{
    public class NonLoggingVortexClient<TEventDto> : INonLoggingVortexClient<TEventDto> where TEventDto : IEventDto
    {
        private static readonly Uri BaseUrl = new Uri("https://vortex.incub8.de/api/event/");
        private readonly VortexConfiguration _configuration;

        public NonLoggingVortexClient(VortexConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendBulkEventAsync(TEventDto[] eventDtos)
        {
            var uri = BuildUri();

            var postRequest = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = JsonContent.Create(eventDtos)
            };
            await SendEventAsyncInternal(postRequest);
        }

        public async Task SendEventAsync(TEventDto eventDto)
        {
            var uri = BuildUri();

            var postRequest = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = JsonContent.Create(eventDto)
            };

            await SendEventAsyncInternal(postRequest);
        }

        private async Task SendEventAsyncInternal(HttpRequestMessage request)
        {
            var client = new HttpClient();
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            if (!content.Equals("[]"))
            {
                throw new HttpRequestException("Response not as expected");
            }
        }

        private Uri BuildUri()
        {
            var builder = new UriBuilder(BaseUrl) {Port = 5001};

            var query = HttpUtility.ParseQueryString(builder.Query);
            query["apikey"] = _configuration.ApiKey;
            builder.Query = query.ToString() ?? "";
            return builder.Uri;
        }
    }
}