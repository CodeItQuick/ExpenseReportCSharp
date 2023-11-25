using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using ExpenseReport.Adapter.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Tests.Adapter.WebBlazorServerApp;

namespace Tests.Adapter.WebAPI;

[Collection("AdapterTests")]
public class ExistingExpensesWebApiControllerTests : IClassFixture<TestingWebApiAppFactory<ApiProgram>>
{
    private TestingWebApiAppFactory<ApiProgram> factory;

    public ExistingExpensesWebApiControllerTests(TestingWebApiAppFactory<ApiProgram> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task CanConstructDefaultExpenseServiceAndViewExpenses()
    { 
        var client = CreateClient(new(ClaimTypes.Role, "User"));
        var response = await client.GetAsync($"/Home");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task ForbidsAccessFromNonUserType()
    { 
        var client = CreateClient(new(ClaimTypes.Role, "Not Valid User"));
        var response = await client.GetAsync($"/Home");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
    [Fact]
    public async Task CanCreateNewExpenseReport()
    {
        var client = CreateClient(new(ClaimTypes.Role, "User"));
        var httpContent = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            ["expenseReportDate"] = DateTimeOffset.Parse("01/01/2023").ToString()
        });

        var httpResponseMessage = await client.PostAsync($"/Home/CreateExpenseReport", httpContent);

        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        var data = JsonConvert.DeserializeObject<ExpenseApiView>(httpResponseMessage.Content.ReadAsStringAsync().Result);
        Assert.Equal(1, data.Id);
    }
    
    [Fact]
    public async Task CanAddAnExpenseToANewExpenseReport()
    {
        var client = CreateClient(new(ClaimTypes.Role, "User"));
        var dateTimeOffset = DateTimeOffset.Parse("09/07/2023");
        var createdExpense = await client.PostAsync($"/Home/CreateExpenseReport?expenseReportDate={dateTimeOffset.ToString()}", 
            new StringContent(""));
        var result = createdExpense.Content.ReadAsStringAsync().Result;
        var createdData = JsonConvert.DeserializeObject<ExpenseApiView>(result);
        var httpResponseMessage = await client.PostAsync(
            $"/Home/ExpenseUpdateView?expenseCost=1001&expenseType=BREAKFAST&expenseReportId={createdData.Id}", 
            new StringContent(""));

        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        var data = JsonConvert.DeserializeObject<ExpenseApiView>(httpResponseMessage.Content.ReadAsStringAsync().Result);
        Assert.Equal(dateTimeOffset.ToString(), data.ExpenseDate.ToString());
        Assert.Equal(1001, data.MealExpenses);
        Assert.Equal(1001, data.TotalExpenses);
    }

    private HttpClient CreateClient(Claim claim)
    {
        var token = JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(
            new JwtSecurityToken(
                JwtTokenProvider.Issuer,
                JwtTokenProvider.Issuer,
                new List<Claim> { claim, new(ClaimTypes.NameIdentifier, "abcd-1234") },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: JwtTokenProvider.SigningCredentials
            )
        );
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}