 using Application.Services;
 using Domain;
 using Microsoft.EntityFrameworkCore;

 namespace Tests;

public class ExistingExpensesRepositoryTests
{
    private static ExpensesContext TestDbContextFactory(int id)
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ExpensesContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb-{id}" + Guid.NewGuid())
            .Options;
        var expensesContext = new ExpensesContext(dbContextOptionsBuilder);
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
        
        Assert.Single(expenseReportAggregate.Expenses);
        Assert.Equal(ExpenseType.DINNER, expenseReportAggregate.Expenses.First().ExpenseType);
        Assert.Equal(100, expenseReportAggregate.Expenses.First().Amount);
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
        
        existingExpensesRepository.ReplaceAllExpenses(new List<Expenses>());
        
        Assert.Null(existingExpensesRepository.GetLastExpenseReport());
    }
    [Fact]
    public void CanCreateNewExpenseReportAggregate()
    {
        var expensesContext = TestDbContextFactory(5);
        var expenseReportAggregate = new ExpenseReportAggregate()
        {
            Expenses = new List<Expenses>()
            {
                new(ExpenseType.DINNER, 100)
            }
        };
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext);

        var addExpenseToReport = existingExpensesRepository.AddAggregate(expenseReportAggregate);

        Assert.NotNull(addExpenseToReport);
    }
}