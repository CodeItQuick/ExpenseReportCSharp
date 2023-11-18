using Application.Adapter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Adapter.WebBlazorServerApp;

public class TestingWebApiAppFactory: WebApplicationFactory<ApiProgram>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var sp = services.BuildServiceProvider();
            services.AddScoped<IExistingExpensesRepository, FakeAPIApplicationRepository>();
            sp.CreateScope();
        });
    }
}