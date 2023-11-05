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
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var expensesContext = new ExpensesContext(dbContextOptionsBuilder);
        var expensesList = expensesContext.Expenses.ToList();
        expensesContext.Expenses.RemoveRange(expensesList);
        var expenseReportAggregates = expensesContext.ExpenseReportAggregates.ToList();
        expensesContext.ExpenseReportAggregates.RemoveRange(expenseReportAggregates);
        expensesContext.SaveChanges();
        
        var existingExpensesRepository = new Application.Services.ExistingExpensesRepository(expensesContext);

        var expenseReportAggregate = existingExpensesRepository.GetLastExpenseReport();
        
        Assert.Null(expenseReportAggregate);
    }
}