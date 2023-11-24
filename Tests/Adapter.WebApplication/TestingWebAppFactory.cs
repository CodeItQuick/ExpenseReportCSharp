using System.Security.Claims;
using Application.Adapter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Adapter.WebApplication;

public class TestingWebAppFactory<T>: WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ExpensesDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<ExpensesDbContext>(options =>
            {
                options.UseInMemoryDatabase("testing_blog-application-3");
            });
            // antiforgery
            // services.AddScoped<IExistingExpensesRepository, FakeWebApplicationRepository>();
            services.AddAntiforgery(t =>
            {
                t.Cookie.Name = AntiForgeryTokenExtractor.AntiForgeryCookieName;
                t.FormFieldName = AntiForgeryTokenExtractor.AntiForgeryFieldName;
            });
            
            services.AddAuthentication(defaultScheme: "TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "TestScheme", options => { });
            
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            using (var appContext = scope.ServiceProvider.GetRequiredService<ExpensesDbContext>())
            {
                try
                {
                    appContext.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    //Log errors or do anything you think it's needed
                    throw;
                }
            }
        });
    }
}