using Application.Adapter;
using Domain;
using ExpenseReport.ApplicationServices;
using Microsoft.EntityFrameworkCore;

namespace Tests.Adapter.Infrastructure;

public class ExistingExpensesRepositoryTests
{
    private static ExpensesDbContext TestDbContextFactory()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb" + Guid.NewGuid())
            .Options;
        var expensesContext = new ExpensesDbContext(dbContextOptionsBuilder);
        var expenseReportAggregates = expensesContext.ExpenseReport.ToList();
        expensesContext.ExpenseReport.RemoveRange(expenseReportAggregates);
        expensesContext.SaveChanges();
        expensesContext.ChangeTracker.Clear();
        return expensesContext;
    }

    [Fact]
    public void CanRetrieveAnEmptyExpenseReport()
    {
        var expensesContext = TestDbContextFactory();
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext);

        var expenseReportAggregate = existingExpensesRepository.RetrieveById(1, "1234-abcd");
        
        Assert.Null(expenseReportAggregate);
    }

    [Fact]
    public void CanRetrieveAFilledExpenseReport()
    {
        var expensesContext = TestDbContextFactory();
        expensesContext.ExpenseReport.Add(new Application.Adapter.ExpenseReport()
        {
            EmployeeId = "1234-abcd"
        });
        expensesContext.SaveChanges();
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext);

        var expenseReportAggregate = existingExpensesRepository.RetrieveById(1, "1234-abcd");
        
        Assert.NotNull(expenseReportAggregate);
    }
    [Fact]
    public void CanRetrieveAFilledExpenseReportWithExpenses()
    {
        var expensesContext = TestDbContextFactory();
        expensesContext.ExpenseReport.Add(new Application.Adapter.ExpenseReport() {
           Expenses = new List<ExpenseDbo>() { new() {  ExpenseType = ExpenseType.DINNER, Amount = 100} },
           ExpenseReportDate = new RealDateProvider().CurrentDate(),
           EmployeeId = "1234-abcd"
        });
        expensesContext.SaveChanges();
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext);

        var expenseReportAggregate = existingExpensesRepository.RetrieveById(1, "1234-abcd");
        
        Assert.Single(expenseReportAggregate.CalculateIndividualExpenses());
    }
    [Fact]
    public void CanCreateNewExpenseReportAggregate()
    {
        var expensesContext = TestDbContextFactory();
        var expenses = new List<CreateExpenseRequest>()
        {
            new() { type = ExpenseType.DINNER, amount = 100 }
        };
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext);

        var addExpenseToReport = existingExpensesRepository.CreateAggregate(new RealDateProvider().CurrentDate(), expenses, "abcd-1234");

        Assert.NotNull(addExpenseToReport);
    }
    [Fact]
    public void CanListAllAggregateExpenseReportsWhenThereAreNoExpenseReports()
    {
        var expensesContext = TestDbContextFactory();
        var expenseReportAggregates = new List<Application.Adapter.ExpenseReport>();
        expensesContext.ExpenseReport.AddRange(expenseReportAggregates);
        expensesContext.SaveChanges();
        expensesContext.ChangeTracker.Clear();
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext);

        var addExpenseToReport = existingExpensesRepository.ListAllExpenseReports("abcd-1234");

        Assert.Empty(addExpenseToReport);
    }
    [Fact]
    public void CanListAllAggregateExpenseReportsWhenThereAreNoExpenses()
    {
        var expensesContext = TestDbContextFactory();
        var expenseReportAggregates = new List<Application.Adapter.ExpenseReport>()
        {
            new()
            {
                Expenses = null,
                ExpenseReportDate = DateTimeOffset.Now,
                EmployeeId = "1234-abcd"
            }
        };
        expensesContext.ExpenseReport.AddRange(expenseReportAggregates);
        expensesContext.SaveChanges();
        expensesContext.ChangeTracker.Clear();
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext);

        var addExpenseToReport = existingExpensesRepository.ListAllExpenseReports("1234-abcd");

        Assert.Single(addExpenseToReport);
        Assert.Empty(addExpenseToReport.First().CalculateIndividualExpenses());
    }
    [Fact]
    public void CanListAllAggregateExpenseReports()
    {
        var expensesContext = TestDbContextFactory();
        var expenseReportAggregates = new List<Application.Adapter.ExpenseReport>()
        {
            new()
            {
                Expenses = new List<ExpenseDbo>()
                {
                    new() { ExpenseType = ExpenseType.DINNER, Amount = 100}
                },
                ExpenseReportDate = DateTimeOffset.Now,
                EmployeeId = "1234-abcd"
            }
        };
        expensesContext.ExpenseReport.AddRange(expenseReportAggregates);
        expensesContext.SaveChanges();
        expensesContext.ChangeTracker.Clear();
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext);

        var addExpenseToReport = existingExpensesRepository.ListAllExpenseReports("1234-abcd");

        Assert.Single(addExpenseToReport);
        Assert.Single(addExpenseToReport.First().CalculateIndividualExpenses());
    }
}