using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Tests;

public class TestingWebAppFactory<T>: WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // database
            // var dbContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            // if (dbContext != null)
            //     services.Remove(dbContext);

            // var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            //
            // services.AddDbContext<AppDbContext>(options =>
            // {
            //     options.UseInMemoryDatabase("InMemoryEmployeeTest");
            //     options.UseInternalServiceProvider(serviceProvider);
            // });

            // antiforgery
            // services.AddAntiforgery(t =>
            // {
            //     t.Cookie.Name = AntiForgeryTokenExtractor.Cookie;
            //     t.FormFieldName = AntiForgeryTokenExtractor.Field;
            // });

            var sp = services.BuildServiceProvider();
            sp.CreateScope();

            // using (var scope = sp.CreateScope())
            // {
            //     using (var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
            //     {
            //         try
            //         {
            //             appContext.Database.EnsureCreated();
            //         }
            //         catch (Exception ex)
            //         {
            //             //Log errors or do anything you think it's needed
            //             throw;
            //         }
            //     }
            // }
        });
    }
}