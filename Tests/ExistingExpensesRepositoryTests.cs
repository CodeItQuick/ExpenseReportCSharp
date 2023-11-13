 using Application.Adapter;
 using Application.Services;
 using Domain;
 using Microsoft.EntityFrameworkCore;
 using WebApplication1.Controllers;
 using Expense = Application.Adapter.Expense;
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
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext, new RealDateProvider());

        var expenseReportAggregate = existingExpensesRepository.GetLastExpenseReport();
        
        Assert.Null(expenseReportAggregate);
    }

    [Fact]
    public void CanRetrieveAFilledExpenseReport()
    {
        var expensesContext = TestDbContextFactory(2);
        expensesContext.ExpenseReportAggregates.Add(new ExpenseReportAggregate());
        expensesContext.SaveChanges();
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext, new RealDateProvider());

        var expenseReportAggregate = existingExpensesRepository.GetLastExpenseReport();
        
        Assert.NotNull(expenseReportAggregate);
    }
    [Fact]
    public void CanRetrieveAFilledExpenseReportWithExpenses()
    {
        var expensesContext = TestDbContextFactory(3);
        expensesContext.ExpenseReportAggregates.Add(new ExpenseReportAggregate() {
           Expenses = new List<Expense>() { new() {  ExpenseType = ExpenseType.DINNER, Amount = 100} },
           ExpenseReportDate = new RealDateProvider().CurrentDate()
        });
        expensesContext.SaveChanges();
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext, new RealDateProvider());

        var expenseReportAggregate = existingExpensesRepository.GetLastExpenseReport();
        
        Assert.Single(expenseReportAggregate.CalculateIndividualExpenses());
        Assert.Equal("DINNER\t100\t ", expenseReportAggregate.CalculateIndividualExpenses().First());
        Assert.Equal("DINNER\t100\t ", expenseReportAggregate.CalculateIndividualExpenses().First());
    }
    [Fact]
    public void CanCreateNewExpenseReportAggregate()
    {
        var expensesContext = TestDbContextFactory(5);
        var expenses = new List<Expense>()
        {
            new() { ExpenseType = ExpenseType.DINNER, Amount = 100}
        };
        var existingExpensesRepository = new ExistingExpensesRepository(expensesContext, new RealDateProvider());

        var addExpenseToReport = existingExpensesRepository.CreateAggregate(expenses, new RealDateProvider().CurrentDate());

        Assert.NotNull(addExpenseToReport);
    }
}