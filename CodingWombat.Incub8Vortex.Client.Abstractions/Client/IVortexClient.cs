using System.Threading.Tasks;
using CodingWombat.Incub8Vortex.Client.Abstractions.DTO;

namespace CodingWombat.Incub8Vortex.Client.Abstractions.Client
{
    public interface IVortexClient<in TEventDto> where TEventDto : IEventDto
    {
        public Task SendEventAsync(TEventDto eventDto);
    }
}