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
            new ExistingExpensesRepository(new RealDateProvider()));

        var viewExpenses = expensesService.RetrieveExpenseReport();
        
        Assert.NotNull(viewExpenses);
    }
    [Fact]
    public void CanViewEmptyExpenseList()
    {
        var expensesList = new List<Expense>();
        var existingExpensesContext = TestDbContextFactory(7);
        ExistingExpensesRepository existingExpensesRepository = new ExistingExpensesRepository(new RealDateProvider());
        var expensesService = new ExpensesService(
            (IDateProvider)new FakeDateProvider(DateTimeOffset.Parse("2023-01-01")), 
            (IExistingExpensesRepository)existingExpensesRepository);

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
            (IDateProvider)new FakeDateProvider(DateTimeOffset.Parse("2023-01-01")), 
            existingExpensesRepository);

        var expenseReport = expensesService.RetrieveExpenseReport();
        
        Assert.Single(expenseReport.CalculateIndividualExpenses());
    }
    [Fact]
    public void CanCreateExpense()
    {
        var existingExpensesContext = TestDbContextFactory(7);
        ExistingExpensesRepository existingExpensesRepository = new ExistingExpensesRepository(new RealDateProvider());
        var expensesService = new ExpensesService(
            (IDateProvider)new FakeDateProvider(DateTimeOffset.Parse("2023-01-01")), 
            (IExistingExpensesRepository)existingExpensesRepository);

        var expenseReport = expensesService.CreateExpense(
            new Expense(ExpenseType.DINNER, 100), DateTimeOffset.Parse("2023-11-09"));
        
        Assert.Single(expenseReport.CalculateIndividualExpenses());
    }
    [Fact]
    public void CanCreateExpenseReport()
    {
        var existingExpensesContext = TestDbContextFactory(7);
        ExistingExpensesRepository existingExpensesRepository = new ExistingExpensesRepository(
            existingExpensesContext, new RealDateProvider());
        var expenseReportDate = DateTimeOffset.Parse("2023-01-01");
        var expensesService = new ExpensesService(
            new FakeDateProvider(expenseReportDate), 
            existingExpensesRepository);

        var expenseReport = expensesService.CreateExpenseReport(expenseReportDate);
        
        Assert.Equal(expenseReportDate, expenseReport.RetrieveDate());
    }
    [Fact]
    public void CanAddAnExpenseToAnExpenseReport()
    {
        var existingExpensesContext = TestDbContextFactory(7);
        ExistingExpensesRepository existingExpensesRepository = new ExistingExpensesRepository(
            existingExpensesContext, new RealDateProvider());
        var expenseReportDate = DateTimeOffset.Parse("2023-01-01");
        var expensesService = new ExpensesService(
            new FakeDateProvider(expenseReportDate), 
            existingExpensesRepository);
        var expenseReport = expensesService.CreateExpenseReport(expenseReportDate);

        var expense = expensesService.CreateExpense(expenseReport.Id, new Expense(ExpenseType.BREAKFAST, 1234));

        Assert.Equal(expenseReport.Id, expense.Id);
    }
    [Fact]
    public void CanAddTwoExpensesToAnExpenseReport()
    {
        var existingExpensesContext = TestDbContextFactory(7);
        ExistingExpensesRepository existingExpensesRepository = new ExistingExpensesRepository(
            existingExpensesContext, new RealDateProvider());
        var expenseReportDate = DateTimeOffset.Parse("2023-01-01");
        var expensesService = new ExpensesService(
            new FakeDateProvider(expenseReportDate), 
            existingExpensesRepository);
        var expenseReport = expensesService.CreateExpenseReport(expenseReportDate);
        expensesService.CreateExpense(expenseReport.Id, new Expense(ExpenseType.BREAKFAST, 1234));

        var expense = expensesService.CreateExpense(expenseReport.Id, new Expense(ExpenseType.BREAKFAST, 1234));

        Assert.Equal(2, expense.CalculateIndividualExpenses().Count);
    }
}