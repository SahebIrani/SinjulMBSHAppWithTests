using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using SinjulMSBH.WebUI.Extensions;

namespace SinjulMSBH.WebUI
{
    public class Program
    {
        public static async Task Main(string[] args) => await CreateHostBuilder(args).Build().RunWithMigrateDatabase();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
