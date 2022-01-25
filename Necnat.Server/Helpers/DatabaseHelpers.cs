using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Necnat.Server.Helpers
{
    public static class DatabaseHelpers
    {
        public static async Task EnsureCreatedAsync<TDbContext>(IHost host) where TDbContext : DbContext
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                await EnsureCreatedAsync<TDbContext>(services);
            }
        }

        public static async Task EnsureCreatedAsync<TDbContext>(IServiceProvider services) where TDbContext : DbContext
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<TDbContext>())
                {
                    await context.Database.EnsureCreatedAsync();
                }
            }
        }
    }
}
