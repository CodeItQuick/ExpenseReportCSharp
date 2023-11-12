 using Application.Adapter;
 using Application.Services;
 using Domain;
 using ExpenseReport.ApplicationServices;
 using Microsoft.EntityFrameworkCore;
 using WebApplication1.Controllers;

 namespace Tests;

public class ExistingExpensesServiceTests
{

    private static ExpensesDbContext TestDbContextFactory(int id)
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb-{id}" + Guid.NewGuid())
            .Options;
        var expensesContext = new ExpensesDbContext(dbContextOptionsBuilder);
        expensesContext.Database.EnsureCreated();
        return expensesContext;
    }
    
    [Fact]
    public void CanConstructDefaultExpenseServiceAndViewExpenses()
    {
        var expensesService = new ExpensesService(
            new FakeDateProvider(DateTimeOffset.Parse("2023-01-01")), 
            new ExistingExpensesRepository());

        var viewExpenses = expensesService.RetrieveExpenseReport();
        
        Assert.NotNull(viewExpenses);
    }
    [Fact]
    public void CanViewEmptyExpenseList()
    {
        var expensesList = new List<Expense>();
        var existingExpensesContext = TestDbContextFactory(7);
        ExistingExpensesRepository existingExpensesRepository = new ExistingExpensesRepository();
        var expensesService = new ExpensesService(
            new FakeDateProvider(DateTimeOffset.Parse("2023-01-01")), 
            expensesList, 
            existingExpensesRepository);

        var expenseReport = expensesService.RetrieveExpenseReport();
        
        Assert.Empty(expenseReport.CalculateIndividualExpenses());
    }
    [Fact]
    public void CanViewExpenseList()
    {
        var expensesList = new List<Expense>()
        {
            new(ExpenseType.DINNER, 1000)
        };
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository(expensesList);
        var expensesService = new ExpensesService(
            new FakeDateProvider(DateTimeOffset.Parse("2023-01-01")), 
            expensesList, 
            existingExpensesRepository);

        var expenseReport = expensesService.RetrieveExpenseReport();
        
        Assert.Single(expenseReport.CalculateIndividualExpenses());
    }
    [Fact]
    public void CanCreateExpense()
    {
        var existingExpensesContext = TestDbContextFactory(7);
        ExistingExpensesRepository existingExpensesRepository = new ExistingExpensesRepository();
        var expensesService = new ExpensesService(
            new FakeDateProvider(DateTimeOffset.Parse("2023-01-01")), 
            new List<Expense>(), 
            existingExpensesRepository);

        var expenseReport = expensesService.CreateExpense(
            100, ExpenseType.DINNER, DateTimeOffset.Parse("2023-11-09"));
        
        Assert.Single(expenseReport.CalculateIndividualExpenses());
    }

}