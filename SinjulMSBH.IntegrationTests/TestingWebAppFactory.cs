using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SinjulMSBH.WebUI;
using SinjulMSBH.WebUI.Data;

namespace SinjulMSBH.IntegrationTests
{
    public class TestingWebAppFactory<T> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                ServiceProvider serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

                services.AddDbContext<ApplicationDbContext>(optionsAction =>
                {
                    optionsAction.UseInMemoryDatabase(nameof(ApplicationDbContext));
                    optionsAction.UseInternalServiceProvider(serviceProvider);
                });

                //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                //    .AddEntityFrameworkStores<ApplicationDbContext>();

                services.AddAntiforgery(setupAction =>
                {
                    setupAction.Cookie.Name = AntiForgeryTokenExtractor.AntiForgeryCookieName;
                    setupAction.FormFieldName = AntiForgeryTokenExtractor.AntiForgeryFieldName;
                });

                ServiceProvider sp = services.BuildServiceProvider();

                using IServiceScope scope = sp.CreateScope();
                using ApplicationDbContext appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    appContext.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            });
        }
    }
}
