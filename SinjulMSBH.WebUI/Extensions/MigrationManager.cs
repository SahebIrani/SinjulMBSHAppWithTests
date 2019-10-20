using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SinjulMSBH.WebUI.Data;

namespace SinjulMSBH.WebUI.Extensions
{
    public static class MigrationManager
    {
        public static async Task RunWithMigrateDatabase(this IHost webHost)
        {
            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    IEnumerable<string> getAppliedMigrations = await context.Database.GetAppliedMigrationsAsync();
                    IEnumerable<string> getPendingMigrations = await context.Database.GetPendingMigrationsAsync();
                    if (getPendingMigrations.Count() > 0) // or ! any
                        await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new Exception(ex.Message);
                }
            }

            await webHost.RunAsync();
        }
    }
}
