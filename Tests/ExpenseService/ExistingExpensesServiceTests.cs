using Application.Adapter;
using Domain;
using ExpenseReport.ApplicationServices;
using Microsoft.EntityFrameworkCore;

namespace Tests.Adapter.Infrastructure;

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
        var expensesService = new ExpensesService(new FakeExistingRepository(new List<ExpenseDbo>()));

        var viewExpenses = expensesService.RetrieveExpenseReport(1, "abcd-1234");
        
        Assert.Equal(1, viewExpenses.Id);
        Assert.Equal(0, viewExpenses.CalculateMealExpenses());
        Assert.Equal(0, viewExpenses.CalculateTotalExpenses());
        Assert.NotNull(viewExpenses.RetrieveDate().ToString());
    }
    [Fact]
    public void CanViewEmptyExpenseList()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository(new List<ExpenseDbo>());
        var expensesService = new ExpensesService(existingExpensesRepository);

        var expenseReport = expensesService.RetrieveExpenseReport(1, "abcd-1234");
        
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

        var expenseReport = expensesService.RetrieveExpenseReport(1, "abcd-1234");
        
        Assert.Single(expenseReport.CalculateIndividualExpenses());
    }
    [Fact]
    public void EmployeeCanViewExpenseReports()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository();
        var expensesService = new ExpensesService(existingExpensesRepository);
        var report = expensesService.CreateExpenseReport(DateTimeOffset.Now, "abcd-1234");

        var retrieveExpenseReport = expensesService.RetrieveExpenseReport(report.Id, "abcd-1234");
        Assert.NotNull(retrieveExpenseReport?.Id);
    }
    [Fact]
    public void EmployeeCanNotViewOtherUsersExpenseReports()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository();
        var expensesService = new ExpensesService(existingExpensesRepository);
        var report = expensesService.CreateExpenseReport(DateTimeOffset.Now, "1234-abcd");

        var retrieveExpenseReport = expensesService.RetrieveExpenseReport(report.Id, "abcd-1234");
        Assert.Null(retrieveExpenseReport);
    }
    [Fact]
    public void EmployeeCanCreateExpense()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository();
        var expensesService = new ExpensesService(existingExpensesRepository);
        var report = expensesService.CreateExpenseReport(DateTimeOffset.Now, "abcd-1234");

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

        var expenseReport = expensesService.CreateExpenseReport(expenseReportDate, "abcd-1234");
        
        Assert.Equal(expenseReportDate, expenseReport.RetrieveDate());
    }
    [Fact]
    public void CanAddAnExpenseToAnExpenseReport()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository();
        var expenseReportDate = DateTimeOffset.Parse("2023-01-01");
        var expensesService = new ExpensesService(existingExpensesRepository);
        var expenseReport = expensesService.CreateExpenseReport(expenseReportDate, "abcd-1234");

        var expense = expensesService.AddExpenseToExpenseReport(expenseReport.Id, new List<CreateExpenseRequest>()
        {
            new()
            {
                type = ExpenseType.BREAKFAST,
                amount = 1234,
                expenseReportId = expenseReport.Id
            }
        });

        Assert.Equal(expenseReport.Id, expense.Id);
        Assert.Equal(expenseReport.IsApproved(), false);
    }
    [Fact]
    public void CanRetrieveZeroExpenseReports()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository();
        var expensesService = new ExpensesService(existingExpensesRepository);

        var expense = expensesService.ListAllExpenseReports("abcd-1234");

        Assert.Empty(expense);
    }
    [Fact]
    public void CanRetrieveOneExpenseReports()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository();
        var expenseReportDate = DateTimeOffset.Parse("2023-01-01");
        var expensesService = new ExpensesService(existingExpensesRepository);
        var expenseReport = expensesService.CreateExpenseReport(expenseReportDate, "abcd-1234");
        expensesService.AddExpenseToExpenseReport(expenseReport.Id, new List<CreateExpenseRequest>()
        {
            new()
            {
                type = ExpenseType.BREAKFAST,
                amount = 1234,
                expenseReportId = expenseReport.Id,
            }
        });

        var expense = expensesService.ListAllExpenseReports("abcd-1234");

        Assert.Single(expense);
    }
    [Fact]
    public void CanRetrieveOnlyCurrentUsersExpenseReports()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository();
        var expenseReportDate = DateTimeOffset.Parse("2023-01-01");
        var expensesService = new ExpensesService(existingExpensesRepository);
        var expenseReport = expensesService.CreateExpenseReport(expenseReportDate, "abcd-1234");
        var expenseReportTwo = expensesService.CreateExpenseReport(expenseReportDate, "1234-abcd");
        expensesService.AddExpenseToExpenseReport(expenseReport.Id, new List<CreateExpenseRequest>()
        {
            new()
            {
                type = ExpenseType.BREAKFAST,
                amount = 1234,
                expenseReportId = expenseReport.Id,
            }
        });

        var expense = expensesService.ListAllExpenseReports("abcd-1234");

        Assert.Single(expense);
    }
    [Fact]
    public void CanRetrieveMultipleExpenseReports()
    {
        IExistingExpensesRepository existingExpensesRepository = new FakeExistingRepository();
        var expenseReportDate = DateTimeOffset.Parse("2023-01-01");
        var expensesService = new ExpensesService(existingExpensesRepository);
        expensesService.CreateExpenseReport(expenseReportDate, "abcd-1234");
        expensesService.CreateExpenseReport(expenseReportDate, "abcd-1234");

        var expense = expensesService.ListAllExpenseReports("abcd-1234");

        Assert.Equal(2, expense.Count);
    }
}