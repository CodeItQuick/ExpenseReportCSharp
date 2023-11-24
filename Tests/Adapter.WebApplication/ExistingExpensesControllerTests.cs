using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using Tests.Adapter.WebApplication;

namespace Tests;

public class ExistingExpensesControllerTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private TestingWebAppFactory<Program> factory;

    public ExistingExpensesControllerTests(TestingWebAppFactory<Program> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task CanConstructDefaultExpenseServiceAndViewExpenses()
    {
        using var client = factory.CreateClient();
        var response = await client.GetAsync($"/Home/Index");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CanCreateNewExpenseReport()
    {
        var client = factory.CreateClient();
        var httpContent = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            ["expenseReportDate"] = DateTimeOffset.Parse("01/01/2023").ToString()
        });
        
        var httpResponseMessage = await client.PostAsync($"/Home/CreateExpenseReport", httpContent);

        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        var data = httpResponseMessage.Content.ReadAsStringAsync().Result;
        Assert.Contains($"Expenses {DateTimeOffset.Parse("01/01/2023").ToString()}", data);
    }
    [Fact]
    public async Task CanAddANewExpenseReport()
    {
        var client = factory
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(scheme: "TestScheme");
        
        var initResponse = await client.GetAsync("/Home/Index");
        var dateTimeOffset = DateTimeOffset.Parse("07/07/2023");
        var formModel = new Dictionary<string, string>
        {
            { "expenseReportDate", dateTimeOffset.ToString() }
        };
        var postData = new FormUrlEncodedContent(formModel);
        var createdExpense = await client.PostAsync("/Home/CreateExpenseReport", postData);
        var createdData = createdExpense.Content.ReadAsStringAsync().Result;
        Assert.Equal(HttpStatusCode.OK, createdExpense.StatusCode);
        Assert.Contains($"Expenses {DateTimeOffset.Parse("07/07/2023").ToString()}", createdData);
    }
        [Fact]
    public async Task CanAddAnExpenseToANewExpenseReport()
    {
        var client = factory.CreateClient();

        var initResponse = await client.GetAsync("/Home/CreateExpenseReport");
        var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);

        var postRequest = new HttpRequestMessage(HttpMethod.Post, $"/Home/CreateExpenseReport");
        postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryValues.field).ToString());

        var dateTimeOffset = DateTimeOffset.Parse("07/07/2023");
        var formModel = new Dictionary<string, string>
    {
        { AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryValues.field },
        { "expenseReportDate", dateTimeOffset.ToString() },
        { "Age", "25" }
    };
        postRequest.Content = new FormUrlEncodedContent(formModel);
        var createdExpense = await client.SendAsync(postRequest);
        var createdData = createdExpense.Content.ReadAsStringAsync().Result;
        var httpContent = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            ["expenseCost"] = "1001",
            ["expenseType"] = ExpenseType.BREAKFAST.ToString(),
            ["expenseReportId"] = "2"
        });

        var httpResponseMessage = await client.PostAsync($"/Home/ExpenseUpdateView", httpContent);

        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        var data = httpResponseMessage.Content.ReadAsStringAsync().Result;
        Assert.Contains($"Expenses {dateTimeOffset.ToString()}", data);
        Assert.Contains($"BREAKFAST 1001 OverExpensed", data);
        Assert.Contains($"Meal Expenses: 1001", data);
        Assert.Contains($"Total Expenses: 1001", data);
    }
}