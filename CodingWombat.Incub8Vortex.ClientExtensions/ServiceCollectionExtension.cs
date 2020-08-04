using CodingWombat.Incub8Vortex.Client.Abstractions.Client;
using CodingWombat.Incub8Vortex.Client.Client;
using CodingWombat.Incub8Vortex.Client.Configuration;
using CodingWombat.Incub8Vortex.Client.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddVortexClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<VortexConfiguration>(configuration.GetSection(VortexConfiguration.Name));
            services.TryAddTransient<IVortexClient<EventDto>,VortexClient>();
            
            return services;
        }
    }
}