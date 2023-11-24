using System.Net;
using Domain;
using Tests.Adapter.WebApplication;

namespace Tests.Adapter.WebBlazorServerApp;

[Collection("AdapterTests")]
public class ExistingExpensesBlazorControllerTests : IClassFixture<TestingBlazorWebAppFactory>
{
    private TestingBlazorWebAppFactory factory;

    public ExistingExpensesBlazorControllerTests(TestingBlazorWebAppFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task CanConstructDefaultExpenseServiceAndViewExpenses()
    {
        using var client = factory.CreateClient();
        var response = await client.GetAsync($"/ExpenseReports");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}