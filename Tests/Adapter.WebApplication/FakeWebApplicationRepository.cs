using Application.Adapter;
using ExpenseReport.ApplicationServices;
using Microsoft.EntityFrameworkCore;
using ExpenseReport = Domain.ExpenseReport;

namespace Tests;

public class FakeWebApplicationRepository : ExistingExpensesRepository
{
    public FakeWebApplicationRepository(): base(new RealDateProvider())
    {
        var dbContextOptions = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase($"testing_blog-application-2")
            .Options; 
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
        var expenseReportAggregates = expensesDbContext.ExpenseReport.ToList();
        expensesDbContext.ExpenseReport.RemoveRange(expenseReportAggregates);
        expensesDbContext.SaveChanges();
        expensesDbContext.ChangeTracker.Clear();
    }
    public FakeWebApplicationRepository(List<Expense> expenses): base(new RealDateProvider())
    {
        var dbContextOptions = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase($"testing_blog-application-{Guid.NewGuid()}")
            .Options; 
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
        var expenseReports = expensesDbContext.ExpenseReport.ToList();
        expensesDbContext.ExpenseReport.RemoveRange(expenseReports);
        expensesDbContext.SaveChanges();
        expensesDbContext.ExpenseReport.Add(new Application.Adapter.ExpenseReport()
        {
            Expenses = expenses,
            ExpenseReportDate = DateTimeOffset.Now
        });
        expensesDbContext.SaveChanges();
        expensesDbContext.ChangeTracker.Clear();
    }
    // Used By The Outside Controller Tests - needs a single persistance for all api calls, additive changes/state persisted
    public FakeWebApplicationRepository(IDateProvider dateProvider) : base(dateProvider)
    {
        var dbContextOptions = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase($"testing_blog-application-3")
            .Options; 
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
    }

    public FakeWebApplicationRepository(ExpensesDbContext expensesDbContext, IDateProvider dateProvider) : 
        base(expensesDbContext, dateProvider)
    {
        this.expensesDbContext = expensesDbContext;
        expensesDbContext.Database.EnsureCreated();
    }
}