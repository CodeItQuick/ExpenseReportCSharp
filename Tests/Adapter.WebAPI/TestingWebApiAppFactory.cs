using System.Security.Claims;
using Application.Adapter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Tests.Adapter.WebAPI;
using Tests.Adapter.WebApplication;

namespace Tests.Adapter.WebBlazorServerApp;

public class TestingWebApiAppFactory<T>: WebApplicationFactory<ApiProgram>
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
                options.UseInMemoryDatabase("testing_blog-application-2");
            });
            // antiforgery
            services.AddScoped<IExistingExpensesRepository, FakeAPIApplicationRepository>();
            
            services.Configure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.Configuration = new OpenIdConnectConfiguration
                    {
                        Issuer = JwtTokenProvider.Issuer,
                    };
                    // ValidIssuer and ValidAudience is not required, but it helps to define them as otherwise they can be overriden by for example the `user-jwts` tool which will cause the validation to fail
                    options.TokenValidationParameters.ValidIssuer = JwtTokenProvider.Issuer;
                    options.TokenValidationParameters.ValidAudience = JwtTokenProvider.Issuer;
                    options.Configuration.SigningKeys.Add(JwtTokenProvider.SecurityKey);
                }
            );
            
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