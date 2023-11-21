using Application.Adapter;
using ExpenseReport.ApplicationServices;
using Microsoft.EntityFrameworkCore;
using ExpenseReport = Domain.ExpenseReport;

namespace Tests;

public class FakeExistingRepository : ExistingExpensesRepository
{
    public FakeExistingRepository(): base()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase($"testing_blog-infrastructure-1")
            .Options; 
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
        var expenseReportAggregates = expensesDbContext.ExpenseReport.ToList();
        expensesDbContext.ExpenseReport.RemoveRange(expenseReportAggregates);
        expensesDbContext.SaveChanges();
        expensesDbContext.ChangeTracker.Clear();
    }
    public FakeExistingRepository(List<ExpenseDbo> expenses): base()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase($"testing_blog-infrastructure-{Guid.NewGuid()}")
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
    public FakeExistingRepository(IDateProvider dateProvider) : base()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase($"testing_blog-infrastructure-3")
            .Options; 
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
    }

    public FakeExistingRepository(ExpensesDbContext expensesDbContext, IDateProvider dateProvider) : 
        base(expensesDbContext)
    {
        this.expensesDbContext = expensesDbContext;
        expensesDbContext.Database.EnsureCreated();
    }
}