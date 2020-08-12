using System;
using System.Net.Http;
using System.Threading.Tasks;
using CodingWombat.Incub8Vortex.Client.Abstractions.Client;
using CodingWombat.Incub8Vortex.Client.Abstractions.DTO;
using CodingWombat.Incub8Vortex.Client.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodingWombat.Incub8Vortex.Client.Client
{
    public class VortexClient<TEventDto> : IVortexClient<TEventDto> where TEventDto : IEventDto
    {
        private readonly ILogger<VortexClient<TEventDto>> _logger;
        private readonly INonLoggingVortexClient<TEventDto> _nonLoggingVortexClient;
        
        public VortexClient(ILogger<VortexClient<TEventDto>> logger,
            IOptions<VortexConfiguration> configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _nonLoggingVortexClient = new NonLoggingVortexClient<TEventDto>(configuration.Value);
                
        }

        public async Task SendBulkEventAsync(TEventDto[] eventDtos)
        {
            try
            {
                await _nonLoggingVortexClient.SendBulkEventAsync(eventDtos);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("Event sending failed!", e);
            }
        }

        public async Task SendEventAsync(TEventDto eventDto)
        {
            try
            {
                await _nonLoggingVortexClient.SendEventAsync(eventDto);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("Event sending failed!", e);
            }
        }
    }
}