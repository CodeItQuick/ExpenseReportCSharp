 using System.Net;
 using System.Security.Claims;
 using Application.Services;
 using Microsoft.AspNetCore.Http;
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.Data.Sqlite;
 using Microsoft.EntityFrameworkCore;
 using Microsoft.Extensions.Logging.Abstractions;
 using WebApplication1.Controllers;

 namespace Tests;

 public class ExistingExpensesControllerWithClaimsTests : IClassFixture<HomeControllerFixtures>
 {
     private readonly HomeControllerFixtures _fixture;
     private readonly HomeController _controller;

     public ExistingExpensesControllerWithClaimsTests(HomeControllerFixtures fixture)
     {
         fixture.SeedDatabase();
         _fixture = fixture;
         _controller = new HomeController(
             new NullLogger<HomeController>());
        
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
         var actionResult = _controller.CreateExpense(100, "BREAKFAST", DateTimeOffset.Parse("2023-11-09")) as ViewResult;

         var indexResponseModel = (actionResult?.Model as ExpenseView);
         Assert.NotNull(actionResult);
         Assert.Equal(100, indexResponseModel.MealExpenses);
         Assert.Equal("BREAKFAST	10	 ", indexResponseModel.IndividualExpenses.First());
         Assert.Equal(100, indexResponseModel.TotalExpenses);
     }
 }
 
public class HomeControllerFixtures
{
    public ExpensesContext? StockContextDb;
    private SqliteConnection? _connection;
    public void SeedDatabase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        var options = new DbContextOptionsBuilder<ExpensesContext>().UseSqlite(_connection).Options;

        StockContextDb = new ExpensesContext(options);
        StockContextDb.Database.EnsureCreated();

        StockContextDb.SaveChanges();
    }
}