using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
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
                    IEnumerable<string> getAppliedMigrations = await context.Database.GetAppliedMigrationsAsync();
                    await context.Database.MigrateAsync();
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
