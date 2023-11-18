using Application.Adapter;
using ExpenseReport.Adapter.WebBlazorServerApp.Data;
using Microsoft.Extensions.Logging.Abstractions;

namespace Tests.Adapter.WebBlazorServerApp;

public class ExistingExpensesBlazorControllerWithClaimsTests
{
    private readonly ExpenseReportAdapterService _controller = new(
        new NullLogger<ExpenseReportAdapterService>(),
        new ExpensesService(new FakeBlazorApplicationRepository(new List<Expense>())));

    [Fact]
    public void CanConstructDefaultExpenseServiceAndViewExpenses()
    {
        var responseModel = _controller.OnGet().Result;
        
        Assert.NotNull(responseModel);
        Assert.Equal(0, responseModel.MealExpenses);
        Assert.Equal(new List<string>(), responseModel.IndividualExpenses);
        Assert.Equal(0, responseModel.TotalExpenses);
        Assert.Equal(1, responseModel.ExpenseReportIds.Count);
    }

    [Fact]
    public void CanCreateANewExpense()
    {
        var expenseReport = _controller.CreateExpenseReport(
            DateTimeOffset.Now).Result;
        var indexResponseModel = _controller.ExpenseView(
            100,
            "BREAKFAST",
            expenseReport.Id).Result;

        Assert.NotNull(indexResponseModel);
        Assert.Equal(100, indexResponseModel.MealExpenses);
        Assert.Equal("BREAKFAST	100	 ", indexResponseModel.IndividualExpenses.First());
        Assert.Equal(100, indexResponseModel.TotalExpenses);
        Assert.Equal(2, indexResponseModel.ExpenseReportIds.Count);
        Assert.Equal(new List<int>() { 1, 2 }, indexResponseModel.ExpenseReportIds);
    }

    [Fact]
    public void CanCreateANewEmptyExpenseReport()
    {
        var indexResponseModel = _controller.CreateExpenseReport(DateTimeOffset.Parse("2023-11-09")).Result;

        Assert.NotNull(indexResponseModel);
        Assert.Equal(0, indexResponseModel.MealExpenses);
        Assert.False(indexResponseModel.IndividualExpenses.Any());
        Assert.Equal(0, indexResponseModel.TotalExpenses);
    }

    [Fact]
    public void CanCreateANewEmptyExpenseReportWithTwoExpenses()
    {
        var expenseReport = _controller.CreateExpenseReport(DateTimeOffset.Parse("2023-11-09"));
        
        var unused = _controller.ExpenseView(
            100,
            "BREAKFAST",
            expenseReport.Id).Result;

        var indexResponseModel = _controller.ExpenseView(
            200,
            "BREAKFAST",
            expenseReport.Id).Result;

        Assert.NotNull(indexResponseModel);
        Assert.Equal(2, indexResponseModel.IndividualExpenses.Count);
    }
}