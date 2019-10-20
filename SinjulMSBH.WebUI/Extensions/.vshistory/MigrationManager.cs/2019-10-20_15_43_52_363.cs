using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SinjulMSBH.WebUI.Data;

namespace SinjulMSBH.WebUI.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost webHost)
        {
            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                using ApplicationDbContext appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    await appContext.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    //Log errors or do anything you think it's needed .. !!!!
                    throw;
                }
            }

            return webHost;
        }
    }
}
