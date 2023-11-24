using System.Security.Claims;
using Application.Adapter;
using ExpenseReport.Adapter.WebAPI.Controllers;
using ExpenseReport.ApplicationServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace Tests.Adapter.WebAPI;

[Collection("AdapterTests")]
public class ExistingExpensesWebApiControllerWithClaimsTests
{
    private readonly HomeApiController _controller;

    public ExistingExpensesWebApiControllerWithClaimsTests()
    {
        _controller = new HomeApiController(
            new NullLogger<HomeApiController>(),
            new ExpensesService(new FakeWebApplicationRepository(new List<ExpenseDbo>())));
        
        var claimsIdentity = new ClaimsIdentity(
            new List<Claim>() { new(ClaimTypes.Name, "test_username") },
            "TestAuthType");
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(claimsIdentity)
            }
        };
    }

    [Fact]
    public void CanConstructDefaultExpenseServiceAndViewExpenses()
    {
        var controllerIndex = _controller.Index();

        var responseModel = (controllerIndex as OkObjectResult).Value as ExpenseApiView;
        Assert.NotNull(responseModel);
        Assert.Equal(0, responseModel.MealExpenses);
        Assert.Equal(new List<ExpenseDto>(), responseModel.IndividualExpenses);
        Assert.Equal(0, responseModel.TotalExpenses);
        Assert.Equal(1, responseModel.ExpenseReportIds.Count);
    }

    [Fact]
    public void CanCreateNewExpense()
    {
        var expenseReport = (_controller.CreateExpenseReport(
            DateTimeOffset.Now) as OkObjectResult).Value as ExpenseApiView;
        var indexResponseModel = (_controller.ExpenseView(
            100,
            "BREAKFAST",
            expenseReport.Id) as OkObjectResult).Value as ExpenseApiView;

        Assert.NotNull(indexResponseModel);
        Assert.Equal(100, indexResponseModel.MealExpenses);
        Assert.Equal("BREAKFAST", indexResponseModel.IndividualExpenses.First().ExpenseType);
        Assert.Equal("", indexResponseModel.IndividualExpenses.First().IsOverExpensedMeal);
        Assert.Equal(100, indexResponseModel.IndividualExpenses.First().Amount);
        Assert.Equal(100, indexResponseModel.TotalExpenses);
        Assert.Equal(2, indexResponseModel.ExpenseReportIds.Count);
        Assert.Equal(new List<int>() { 1, 2 }, indexResponseModel.ExpenseReportIds);
    }

    [Fact]
    public void CanCreateANewEmptyExpenseReport()
    {
        var indexResponseModel = (_controller
            .CreateExpenseReport(DateTimeOffset.Parse("2023-11-09")) as OkObjectResult).Value as ExpenseApiView;

        Assert.NotNull(indexResponseModel);
        Assert.Equal(0, indexResponseModel.MealExpenses);
        Assert.False(indexResponseModel.IndividualExpenses.Any());
        Assert.Equal(0, indexResponseModel.TotalExpenses);
    }

    [Fact]
    public void CanCreateANewEmptyExpenseReportWithTwoExpenses()
    {
        var expenseReport = (_controller
            .CreateExpenseReport(DateTimeOffset.Parse("2023-10-09")) as OkObjectResult).Value as ExpenseApiView;
        
        var unused = (_controller.ExpenseView(
            100,
            "BREAKFAST",
            expenseReport.Id) as OkObjectResult).Value as ExpenseApiView;

        var indexResponseModel = (_controller.ExpenseView(
            200,
            "BREAKFAST",
            expenseReport.Id) as OkObjectResult).Value as ExpenseApiView;

        Assert.NotNull(indexResponseModel);
        Assert.Equal(2, indexResponseModel.IndividualExpenses.Count);
    }
}