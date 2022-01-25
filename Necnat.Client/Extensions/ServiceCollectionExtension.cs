using Microsoft.Extensions.DependencyInjection;
using Necnat.Client.Interfaces;
using Necnat.Client.Services;
using Necnat.Shared;
using Necnat.Shared.Interfaces;

namespace Necnat.Client.Extensions
{
    public static partial class ServiceCollectionExtension
    {
        public static IServiceCollection AddNecnatServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<INecnatSessionService, NecnatSessionService>();
            serviceCollection.AddScoped<INecnatSwalService, NecnatSwalService>();
            serviceCollection.UseAllOfType<IValidator>(new[] { typeof(NecnatSharedRef).Assembly });

            return serviceCollection;
        }
    }
}
