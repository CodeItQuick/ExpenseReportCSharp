using System.Security.Claims;
using Application.Adapter;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using WebApplication1.Controllers;

namespace Tests;

public class ExistingExpensesControllerWithClaimsTests
{
 private readonly HomeController _controller;

 public ExistingExpensesControllerWithClaimsTests()
 {
     _controller = new HomeController(
         new NullLogger<HomeController>(), 
             new ExpensesService(
             new RealDateProvider(), 
             new FakeExistingRepository(new List<Expense>())));
    
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
     var actionResult = _controller.Index() as ViewResult;

     var indexResponseModel = (actionResult?.Model as ExpenseView);
     Assert.NotNull(actionResult);
     Assert.Equal(0, indexResponseModel.MealExpenses);
     Assert.Equal(new List<string>(), indexResponseModel.IndividualExpenses);
     Assert.Equal(0, indexResponseModel.TotalExpenses);
 }
 [Fact]
 public void CanCreateANewExpense()
 {
     var actionResult = _controller.ExpenseView(
         100, 
         "BREAKFAST", 
         DateTimeOffset.Parse("2023-11-09")) as ViewResult;

     var indexResponseModel = (actionResult?.Model as ExpenseView);
     Assert.NotNull(actionResult);
     Assert.Equal(100, indexResponseModel.MealExpenses);
     Assert.Equal("BREAKFAST	100	 ", indexResponseModel.IndividualExpenses.First());
     Assert.Equal(100, indexResponseModel.TotalExpenses);
 }
 [Fact]
 public void CanCreateANewEmptyExpenseReport()
 {
     var actionResult = _controller.CreateExpenseReport(DateTimeOffset.Parse("2023-11-09")) as ViewResult;

     var indexResponseModel = (actionResult?.Model as ExpenseView);
     Assert.NotNull(actionResult);
     Assert.Equal(0, indexResponseModel.MealExpenses);
     Assert.False(indexResponseModel.IndividualExpenses.Any());
     Assert.Equal(0, indexResponseModel.TotalExpenses);
 }
 [Fact]
 public void CanCreateANewEmptyExpenseReportWithAnExpense()
 {
     var expenseReport = _controller.CreateExpenseReport(DateTimeOffset.Parse("2023-11-09"));

     var actionResult = _controller.ExpenseView(
         100, 
         "BREAKFAST",
         (expenseReport.Model as ExpenseView).Id) as ViewResult;
     
     var indexResponseModel = (actionResult?.Model as ExpenseView);
     Assert.NotNull(actionResult);
     Assert.Equal(100, indexResponseModel.MealExpenses);
     Assert.Equal("BREAKFAST\t100\t ", indexResponseModel.IndividualExpenses.First());
     Assert.Equal(100, indexResponseModel.TotalExpenses);
 }
 [Fact]
 public void CanCreateANewEmptyExpenseReportWithTwoExpenses()
 {
     var expenseReport = _controller.CreateExpenseReport(DateTimeOffset.Parse("2023-11-09"));
     var expenseReportId = (expenseReport.Model as ExpenseView).Id;
     _controller.ExpenseView(
         100, 
         "BREAKFAST",
         expenseReportId);

     var actionResult = _controller.ExpenseView(
         200, 
         "BREAKFAST",
         expenseReportId) as ViewResult;
     
     var indexResponseModel = (actionResult?.Model as ExpenseView);
     Assert.NotNull(actionResult);
     Assert.Equal(2, indexResponseModel.IndividualExpenses.Count);
 }
}