 using Application.Adapter;
 using Application.Services;
 using Domain;
 using Microsoft.EntityFrameworkCore;
 using WebApplication1.Controllers;
 using ExpenseReport = Domain.ExpenseReport;

 namespace Tests;

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
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext, new RealDateProvider());

        var expenseReportAggregate = existingExpensesRepository.RetrieveById(1);
        
        Assert.Null(expenseReportAggregate);
    }

    [Fact]
    public void CanRetrieveAFilledExpenseReport()
    {
        var expensesContext = TestDbContextFactory();
        expensesContext.ExpenseReport.Add(new Application.Adapter.ExpenseReport());
        expensesContext.SaveChanges();
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext, new RealDateProvider());

        var expenseReportAggregate = existingExpensesRepository.RetrieveById(1);
        
        Assert.NotNull(expenseReportAggregate);
    }
    [Fact]
    public void CanRetrieveAFilledExpenseReportWithExpenses()
    {
        var expensesContext = TestDbContextFactory();
        expensesContext.ExpenseReport.Add(new Application.Adapter.ExpenseReport() {
           Expenses = new List<ExpenseDbo>() { new() {  ExpenseType = ExpenseType.DINNER, Amount = 100} },
           ExpenseReportDate = new RealDateProvider().CurrentDate()
        });
        expensesContext.SaveChanges();
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext, new RealDateProvider());

        var expenseReportAggregate = existingExpensesRepository.RetrieveById(1);
        
        Assert.Single(expenseReportAggregate.CalculateIndividualExpenses());
        Assert.Equal("DINNER\t100\t ", expenseReportAggregate.CalculateIndividualExpenses().First());
        Assert.Equal("DINNER\t100\t ", expenseReportAggregate.CalculateIndividualExpenses().First());
    }
    [Fact]
    public void CanCreateNewExpenseReportAggregate()
    {
        var expensesContext = TestDbContextFactory();
        var expenses = new List<Expense>()
        {
            new(ExpenseType.DINNER, 100) 
        };
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext, new RealDateProvider());

        var addExpenseToReport = existingExpensesRepository.CreateAggregate(expenses, new RealDateProvider().CurrentDate());

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
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext, new RealDateProvider());

        var addExpenseToReport = existingExpensesRepository.ListAllExpenseReports();

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
                ExpenseReportDate = DateTimeOffset.Now
            }
        };
        expensesContext.ExpenseReport.AddRange(expenseReportAggregates);
        expensesContext.SaveChanges();
        expensesContext.ChangeTracker.Clear();
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext, new RealDateProvider());

        var addExpenseToReport = existingExpensesRepository.ListAllExpenseReports();

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
                ExpenseReportDate = DateTimeOffset.Now
            }
        };
        expensesContext.ExpenseReport.AddRange(expenseReportAggregates);
        expensesContext.SaveChanges();
        expensesContext.ChangeTracker.Clear();
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext, new RealDateProvider());

        var addExpenseToReport = existingExpensesRepository.ListAllExpenseReports();

        Assert.Single(addExpenseToReport);
        Assert.Single(addExpenseToReport.First().CalculateIndividualExpenses());
    }
}