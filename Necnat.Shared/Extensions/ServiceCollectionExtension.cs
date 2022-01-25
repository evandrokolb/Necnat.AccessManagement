using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtension
    {
        public static IServiceCollection UseAllOfType<T>(this IServiceCollection serviceCollection, Assembly[] assemblies, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.IsClass && x.GetInterfaces().Contains(typeof(T))));
            foreach (var type in typesFromAssemblies)
                serviceCollection.Add(new ServiceDescriptor(type, type, lifetime));

            return serviceCollection;
        }
    }
}