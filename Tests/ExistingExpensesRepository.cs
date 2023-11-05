 using Application.Services;
 using Domain;
 using ExpenseReportCSharp.Adapter;
 using Microsoft.EntityFrameworkCore;
 using Expenses = Application.Services.Expenses;

 namespace Tests;

public class ExistingExpensesRepository
{
    // Unit Tests on Domain
    [Fact]
    public void CanRetrieveAnEmptyExpenseReport()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ExpensesContext>()
            .UseInMemoryDatabase(databaseName: "TestDb-" + Guid.NewGuid())
            .Options;
        var expensesContext = new ExpensesContext(dbContextOptionsBuilder);
        var existingExpensesRepository = new Application.Services.ExistingExpensesRepository(expensesContext);

        var expenseReportAggregate = existingExpensesRepository.GetLastExpenseReport();
        
        Assert.Null(expenseReportAggregate);
    }
    // Unit Tests on Domain
    [Fact]
    public void CanRetrieveAFilledExpenseReport()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ExpensesContext>()
            .UseInMemoryDatabase(databaseName: "TestDb-" + Guid.NewGuid())
            .Options;
        var expensesContext = new ExpensesContext(dbContextOptionsBuilder);
        expensesContext.ExpenseReportAggregates.Add(new ExpenseReportAggregate());
        expensesContext.SaveChanges();
        var existingExpensesRepository = new Application.Services.ExistingExpensesRepository(expensesContext);

        var expenseReportAggregate = existingExpensesRepository.GetLastExpenseReport();
        
        Assert.NotNull(expenseReportAggregate);
    }
}