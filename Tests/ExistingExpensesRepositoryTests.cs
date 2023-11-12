 using Application.Adapter;
 using Application.Services;
 using Domain;
 using Microsoft.EntityFrameworkCore;
 using WebApplication1.Controllers;
 using ExpenseReport = Domain.ExpenseReport;

 namespace Tests;

public class ExistingExpensesRepositoryTests
{
    private static ExpensesDbContext TestDbContextFactory(int id)
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb-{id}" + Guid.NewGuid())
            .Options;
        var expensesContext = new ExpensesDbContext(dbContextOptionsBuilder);
        return expensesContext;
    }

    [Fact]
    public void CanRetrieveAnEmptyExpenseReport()
    {
        var expensesContext = TestDbContextFactory(1);
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext);

        var expenseReportAggregate = existingExpensesRepository.GetLastExpenseReport();
        
        Assert.Null(expenseReportAggregate);
    }

    [Fact]
    public void CanRetrieveAFilledExpenseReport()
    {
        var expensesContext = TestDbContextFactory(2);
        expensesContext.ExpenseReportAggregates.Add(new ExpenseReportAggregate());
        expensesContext.SaveChanges();
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext);

        var expenseReportAggregate = existingExpensesRepository.GetLastExpenseReport();
        
        Assert.NotNull(expenseReportAggregate);
    }
    [Fact]
    public void CanRetrieveAFilledExpenseReportWithExpenses()
    {
        var expensesContext = TestDbContextFactory(3);
        expensesContext.ExpenseReportAggregates.Add(new ExpenseReportAggregate()
        {
            Id = 1,
            Expenses = new List<Expenses>()
            {
                new(ExpenseType.DINNER, 100) { Id = 1 }
            }
        });
        expensesContext.SaveChanges();
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext);

        var expenseReportAggregate = existingExpensesRepository.GetLastExpenseReport();
        
        Assert.Single(expenseReportAggregate.CalculateIndividualExpenses());
        Assert.Equal("DINNER\t100\t ", expenseReportAggregate.CalculateIndividualExpenses().First());
        Assert.Equal("DINNER\t100\t ", expenseReportAggregate.CalculateIndividualExpenses().First());
    }
    [Fact]
    public void CanReplaceExistingExpensesWithDefaultData()
    {
        var expensesContext = TestDbContextFactory(4);
        expensesContext.ExpenseReportAggregates.Add(new ExpenseReportAggregate()
        {
            Id = 1,
            Expenses = new List<Expenses>()
            {
                new(ExpenseType.DINNER, 100) { Id = 1 }
            }
        });
        expensesContext.SaveChanges();
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext);
        
        existingExpensesRepository.ReplaceAllExpenses(new List<Expense>());
        
        Assert.Null(existingExpensesRepository.GetLastExpenseReport());
    }
    [Fact]
    public void CanCreateNewExpenseReportAggregate()
    {
        var expensesContext = TestDbContextFactory(5);
        var expenseReportAggregate = new Domain.ExpenseReport(
            new List<Expense>()
            {
                new(ExpenseType.DINNER, 100)
            }); // FIXME: borked
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext);

        var addExpenseToReport = existingExpensesRepository.AddAggregate(expenseReportAggregate);

        Assert.NotNull(addExpenseToReport);
    }
}