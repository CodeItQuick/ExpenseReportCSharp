 using Application.Services;
 using Domain;
 using Microsoft.EntityFrameworkCore;

 namespace Tests;

public class ExistingExpensesRepository
{
    private static ExpensesContext TestDbContextFactory()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ExpensesContext>()
            .UseInMemoryDatabase(databaseName: "TestDb-" + Guid.NewGuid())
            .Options;
        var expensesContext = new ExpensesContext(dbContextOptionsBuilder);
        return expensesContext;
    }

    [Fact]
    public void CanRetrieveAnEmptyExpenseReport()
    {
        var expensesContext = TestDbContextFactory();
        var existingExpensesRepository = new Application.Services.ExistingExpensesRepository(expensesContext);

        var expenseReportAggregate = existingExpensesRepository.GetLastExpenseReport();
        
        Assert.Null(expenseReportAggregate);
    }

    [Fact]
    public void CanRetrieveAFilledExpenseReport()
    {
        var expensesContext = TestDbContextFactory();
        expensesContext.ExpenseReportAggregates.Add(new ExpenseReportAggregate());
        expensesContext.SaveChanges();
        var existingExpensesRepository = new Application.Services.ExistingExpensesRepository(expensesContext);

        var expenseReportAggregate = existingExpensesRepository.GetLastExpenseReport();
        
        Assert.NotNull(expenseReportAggregate);
    }
    [Fact]
    public void CanRetrieveAFilledExpenseReportWithExpenses()
    {
        var expensesContext = TestDbContextFactory();
        expensesContext.ExpenseReportAggregates.Add(new ExpenseReportAggregate()
        {
            Expenses = new List<Expenses>()
            {
                new(ExpenseType.DINNER, 100)
            }
        });
        expensesContext.SaveChanges();
        var existingExpensesRepository = new Application.Services.ExistingExpensesRepository(expensesContext);

        var expenseReportAggregate = existingExpensesRepository.GetLastExpenseReport();
        
        Assert.Single(expenseReportAggregate.Expenses);
    }
}