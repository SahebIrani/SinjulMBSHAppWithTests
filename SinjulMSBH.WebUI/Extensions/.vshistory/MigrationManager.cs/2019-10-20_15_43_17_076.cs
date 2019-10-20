using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SinjulMSBH.WebUI.Data;

namespace SinjulMSBH.WebUI.Extensions
{
    public static class MigrationManager
    {
        public static IWebHost MigrateDatabase(this IWebHost webHost)
        {
            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                using (ApplicationDbContext appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            }

            return webHost;
        }
    }
}
