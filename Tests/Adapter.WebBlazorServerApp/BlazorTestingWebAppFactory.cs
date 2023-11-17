using Application.Adapter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Adapter.WebBlazorServerApp;

public class BlazorTestingWebAppFactory: WebApplicationFactory<BlazorProgram>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var sp = services.BuildServiceProvider();
            services.AddScoped<IExistingExpensesRepository, FakeBlazorApplicationRepository>();
            sp.CreateScope();
        });
    }
}