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
        var expensesService = new ExpensesService(new ExistingExpensesRepository(new RealDateProvider()));

        var viewExpenses = expensesService.RetrieveExpenseReport(1);
        
        Assert.Null(viewExpenses);
    }
    [Fact]
    public void CanViewEmptyExpenseList()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository(new List<ExpenseDbo>());
        var expensesService = new ExpensesService(existingExpensesRepository);

        var expenseReport = expensesService.RetrieveExpenseReport(1);
        
        Assert.Empty(expenseReport.CalculateIndividualExpenses());
    }
    [Fact]
    public void CanViewExpenseList()
    {
        var expensesList = new List<ExpenseDbo>()
        {
            new() { ExpenseType = ExpenseType.DINNER, Amount = 1000}
        };
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository(expensesList);
        var expensesService = new ExpensesService(existingExpensesRepository);

        var expenseReport = expensesService.RetrieveExpenseReport(1);
        
        Assert.Single(expenseReport.CalculateIndividualExpenses());
    }
    [Fact]
    public void CanCreateExpense()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository();
        var expensesService = new ExpensesService(existingExpensesRepository);
        var report = expensesService.CreateExpenseReport(DateTimeOffset.Now);

        var expenseReport = expensesService.AddExpenseToExpenseReport(
            report.Id, new List<CreateExpenseRequest>()
            {
                new()
                {
                    type = ExpenseType.DINNER,
                    amount = 100,
                    expenseReportId = report.Id,
                }
            });
        
        Assert.Single(expenseReport.CalculateIndividualExpenses());
    }
    [Fact]
    public void CanCreateExpenseReport()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository();
        var expenseReportDate = DateTimeOffset.Parse("2023-01-01");
        var expensesService = new ExpensesService(existingExpensesRepository);

        var expenseReport = expensesService.CreateExpenseReport(expenseReportDate);
        
        Assert.Equal(expenseReportDate, expenseReport.RetrieveDate());
    }
    [Fact]
    public void CanAddAnExpenseToAnExpenseReport()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository();
        var expenseReportDate = DateTimeOffset.Parse("2023-01-01");
        var expensesService = new ExpensesService(existingExpensesRepository);
        var expenseReport = expensesService.CreateExpenseReport(expenseReportDate);

        var expense = expensesService.AddExpenseToExpenseReport(expenseReport.Id, new List<CreateExpenseRequest>()
        {
            new()
            {
                type = ExpenseType.BREAKFAST,
                amount = 1234,
                expenseReportId = expenseReport.Id,
            }
        });

        Assert.Equal(expenseReport.Id, expense.Id);
    }
    [Fact]
    public void CanRetrieveZeroExpenseReports()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository();
        var expensesService = new ExpensesService(existingExpensesRepository);

        var expense = expensesService.ListAllExpenseReports();

        Assert.Empty(expense);
    }
    [Fact]
    public void CanRetrieveOneExpenseReports()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository();
        var expenseReportDate = DateTimeOffset.Parse("2023-01-01");
        var expensesService = new ExpensesService(existingExpensesRepository);
        var expenseReport = expensesService.CreateExpenseReport(expenseReportDate);
        expensesService.AddExpenseToExpenseReport(expenseReport.Id, new List<CreateExpenseRequest>()
        {
            new()
            {
                type = ExpenseType.BREAKFAST,
                amount = 1234,
                expenseReportId = expenseReport.Id,
            }
        });

        var expense = expensesService.ListAllExpenseReports();

        Assert.Single(expense);
    }
    [Fact]
    public void CanRetrieveMultipleExpenseReports()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository();
        var expenseReportDate = DateTimeOffset.Parse("2023-01-01");
        var expensesService = new ExpensesService(existingExpensesRepository);
        expensesService.CreateExpenseReport(expenseReportDate);
        expensesService.CreateExpenseReport(expenseReportDate);

        var expense = expensesService.ListAllExpenseReports();

        Assert.Equal(2, expense.Count);
    }
}