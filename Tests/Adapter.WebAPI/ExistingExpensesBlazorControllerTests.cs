using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Domain;
using ExpenseReport.Adapter.WebAPI.Controllers;
using ExpenseReport.Adapter.WebBlazorServerApp.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Tests.Adapter.WebApplication;

namespace Tests.Adapter.WebBlazorServerApp;

public class ExistingExpensesWebApiControllerTests : IClassFixture<TestingWebApiAppFactory>
{
    private TestingWebApiAppFactory factory;

    public ExistingExpensesWebApiControllerTests(TestingWebApiAppFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task CanConstructDefaultExpenseServiceAndViewExpenses()
    {
        using var client = factory.CreateClient();
        var response = await client.GetAsync($"/Home");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task CanCreateNewExpenseReport()
    {
        using var client = factory.CreateClient();
        var httpContent = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            ["expenseReportDate"] = DateTimeOffset.Parse("01/01/2023").ToString(),
        });

        var httpResponseMessage = await client.PostAsync($"/Home/CreateExpenseReport", httpContent);

        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        var data = JsonConvert.DeserializeObject<ExpenseApiView>(httpResponseMessage.Content.ReadAsStringAsync().Result);
        Assert.Equal(1, data.Id);
    }
    
    [Fact]
    public async Task CanAddAnExpenseToANewExpenseReport()
    {
        using var client = factory.CreateClient();
        var dateTimeOffset = DateTimeOffset.Parse("09/07/2023");
        var createdExpense = await client.PostAsync($"/Home/CreateExpenseReport?expenseReportDate={dateTimeOffset.ToString()}", 
            new StringContent(""));
        var createdData = JsonConvert.DeserializeObject<ExpenseApiView>(createdExpense.Content.ReadAsStringAsync().Result);
        var httpResponseMessage = await client.PostAsync(
            $"/Home/ExpenseUpdateView?expenseCost=1001&expenseType=BREAKFAST&expenseReportId={createdData.Id}", 
            new StringContent(""));

        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        var data = JsonConvert.DeserializeObject<ExpenseApiView>(httpResponseMessage.Content.ReadAsStringAsync().Result);
        Assert.Equal(dateTimeOffset.ToString(), data.ExpenseDate.ToString());
        Assert.Equal(1001, data.MealExpenses);
        Assert.Equal(1001, data.TotalExpenses);
    }
}