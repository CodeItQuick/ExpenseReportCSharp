 using Application.Services;
 using Domain;
 using Microsoft.EntityFrameworkCore;

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
        var expensesService = new ExpensesService(new FakeDateProvider(DateTimeOffset.Now));

        var viewExpenses = expensesService.RetrieveExpenseReport();
        
        Assert.NotNull(viewExpenses);
    }
    [Fact]
    public void CanViewEmptyExpenseList()
    {
        var expensesList = new List<Expenses>();
        var existingExpensesContext = TestDbContextFactory(7);
        var existingExpensesRepository = new ExistingExpensesRepository(existingExpensesContext);
        var expensesService = new ExpensesService(
            new FakeDateProvider(DateTimeOffset.Now), 
            expensesList, 
            existingExpensesRepository);

        var expenseReport = expensesService.RetrieveExpenseReport();
        
        Assert.Empty(expenseReport.CalculateIndividualExpenses());
    }
    [Fact]
    public void CanViewExpenseList()
    {
        var expensesList = new List<Expenses>()
        {
            new(ExpenseType.DINNER, 1000)
        };
        var existingExpensesContext = TestDbContextFactory(7);
        var existingExpensesRepository = new ExistingExpensesRepository(existingExpensesContext);
        var expensesService = new ExpensesService(
            new FakeDateProvider(DateTimeOffset.Now), 
            expensesList, 
            existingExpensesRepository);

        var expenseReport = expensesService.RetrieveExpenseReport();
        
        Assert.Single(expenseReport.CalculateIndividualExpenses());
    }
    [Fact]
    public void CanCreateExpense()
    {
        var existingExpensesContext = TestDbContextFactory(7);
        var existingExpensesRepository = new ExistingExpensesRepository(existingExpensesContext);
        var expensesService = new ExpensesService(
            new FakeDateProvider(DateTimeOffset.Now), 
            new List<Expenses>(), 
            existingExpensesRepository);

        var expenseReport = expensesService.CreateExpense(
            100, ExpenseType.DINNER, DateTimeOffset.Parse("2023-11-09"));
        
        Assert.Single(expenseReport.CalculateIndividualExpenses());
    }

}