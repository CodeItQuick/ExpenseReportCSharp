using System.Net;
using Domain;

namespace Tests.Adapter.WebApplication;

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
    public async Task CanAddAnExpenseToNewExpenseReport()
    {
        var client = factory.CreateClient();

        var reportTime = DateTimeOffset.Parse("07/07/2023");
        var postContent = new FormUrlEncodedContent(
            new Dictionary<string, string>
        {
            { "expenseReportDate", reportTime.ToString() }
        });
        var createdExpense = await client.PostAsync("/Home/CreateExpenseReport", postContent);
        var createdData = createdExpense.Content.ReadAsStringAsync().Result;
        var httpContent = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            ["expenseCost"] = "1001",
            ["expenseType"] = ExpenseType.BREAKFAST.ToString(),
            ["expenseReportId"] = "1"
        });

        var httpResponseMessage = await client.PostAsync($"/Home/ExpenseUpdateView", httpContent);

        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        var data = httpResponseMessage.Content.ReadAsStringAsync().Result;
        Assert.Contains($"Expenses {reportTime.Month.ToString()}/{reportTime.Day.ToString()}/{reportTime.Year.ToString()}", data);
        Assert.Contains($"BREAKFAST 1001 OverExpensed", data);
        Assert.Contains($"Meal Expenses: 1001", data);
        Assert.Contains($"Total Expenses: 1001", data);
    }
}