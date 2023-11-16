using Application.Adapter;
using ExpenseReport.ApplicationServices;
using Microsoft.EntityFrameworkCore;
using ExpenseReport = Domain.ExpenseReport;

namespace Tests;

public class FakeExistingRepository : ExistingExpensesRepository
{
    public FakeExistingRepository(): base(new RealDateProvider())
    {
        var dbContextOptions = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase($"testing_blog-1")
            .Options; 
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
    }
    public FakeExistingRepository(List<Expense> expenses): base(new RealDateProvider())
    {
        var dbContextOptions = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase($"testing_blog-{Guid.NewGuid()}")
            .Options; 
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
        var expenseReports = expensesDbContext.ExpenseReportAggregates.ToList();
        expensesDbContext.ExpenseReportAggregates.RemoveRange(expenseReports);
        expensesDbContext.SaveChanges();
        expensesDbContext.ExpenseReportAggregates.Add(new ExpenseReportAggregate()
        {
            Expenses = expenses,
            ExpenseReportDate = DateTimeOffset.Now
        });
        expensesDbContext.SaveChanges();
        expensesDbContext.ChangeTracker.Clear();
    }
    // Used By The Outside Controller Tests - needs a single persistance for all api calls, additive changes/state persisted
    public FakeExistingRepository(IDateProvider dateProvider) : base(dateProvider)
    {
        var dbContextOptions = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase($"testing_blog-3")
            .Options; 
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
    }

    public FakeExistingRepository(ExpensesDbContext expensesDbContext, IDateProvider dateProvider) : 
        base(expensesDbContext, dateProvider)
    {
        this.expensesDbContext = expensesDbContext;
        expensesDbContext.Database.EnsureCreated();
    }
}