using System.Threading.Tasks;
using CodingWombat.Incub8Vortex.Client.Abstractions.DTO;

namespace CodingWombat.Incub8Vortex.Client.Abstractions.Client
{
    public interface INonLoggingVortexClient<in TEventDto> where TEventDto : IEventDto
    {
        public Task SendBulkEventAsync(TEventDto[] eventDtos);
        public Task SendEventAsync(TEventDto eventDto);
    }
}