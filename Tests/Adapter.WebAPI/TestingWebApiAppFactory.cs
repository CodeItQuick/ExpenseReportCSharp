using System.Security.Claims;
using System.Text;
using Application.Adapter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
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
            
            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            // services.AddAuthorization(o =>
            // {
            //     o.AddPolicy("AllRegisteredUsers", policy => 
            //         policy.RequireClaim(ClaimTypes.Role, "User"));
            // });
            //
            services.Configure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                opt =>
                {
                    opt.Audience = "api://local-unit-test";
                    opt.RequireHttpsMetadata = false;
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ClockSkew = TokenValidationParameters.DefaultClockSkew,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = JwtTokenProvider.Issuer,
                        ValidIssuer = JwtTokenProvider.Issuer,
                        IssuerSigningKey = JwtTokenProvider.SecurityKey
                    };
                }
            );
            services.AddAuthorization(o =>
            {
                o.AddPolicy("AllRegisteredUsers", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
            });

            
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