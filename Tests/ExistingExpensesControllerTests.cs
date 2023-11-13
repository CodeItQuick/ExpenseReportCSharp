using System.Net;
using Domain;

namespace Tests;

public class ExistingExpensesControllerTests : IClassFixture<TestingWebAppFactory>
{
    private TestingWebAppFactory factory;

    public ExistingExpensesControllerTests(TestingWebAppFactory factory)
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
        using var client = factory.CreateClient();
        var httpContent = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            ["expenseReportDate"] = DateTimeOffset.Parse("01/01/2023").ToString(),
        });

        var httpResponseMessage = await client.PostAsync($"/Home/CreateExpenseReport", httpContent);

        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        var data = httpResponseMessage.Content.ReadAsStringAsync().Result;
        Assert.Contains($"Expenses {DateTimeOffset.Parse("01/01/2023").ToString()}", data);
    }

    [Fact]
    public async Task CanAddAnExpenseToANewExpenseReport()
    {
        using (var client = factory.CreateClient())
        {
            var dateTimeOffset = DateTimeOffset.Parse("07/07/2023");
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["expenseReportDate"] = dateTimeOffset.ToString(),
            });
            var createdExpense = await client.PostAsync($"/Home/CreateExpenseReport", content);
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
        }
    }
}