using Application.Adapter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Adapter.WebApplication;

public class TestingWebAppFactory: WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var sp = services.BuildServiceProvider();
            services.AddScoped<IExistingExpensesRepository, FakeWebApplicationRepository>();
            sp.CreateScope();
        });
    }
}