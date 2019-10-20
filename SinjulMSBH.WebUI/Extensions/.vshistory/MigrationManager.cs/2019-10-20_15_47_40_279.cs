using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SinjulMSBH.WebUI.Data;

namespace SinjulMSBH.WebUI.Extensions
{
    public static class MigrationManager
    {
        public static async Task<IHost> MigrateDatabaseAsync(this IHost webHost)
        {
            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    await context.Database.EnsureCreatedAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new Exception(ex.Message);
                }
            }

            return webHost;
        }
    }
}
