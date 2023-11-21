using Application.Adapter;
using ExpenseReport.ApplicationServices;
using Microsoft.EntityFrameworkCore;

namespace Tests.Adapter.WebBlazorServerApp;

public class FakeAPIApplicationRepository : ExistingExpensesRepository
{
    public FakeAPIApplicationRepository(): base()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase($"testing_blog-blazor")
            .Options; 
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
        var expenseReportAggregates = expensesDbContext.ExpenseReport.ToList();
        expensesDbContext.ExpenseReport.RemoveRange(expenseReportAggregates);
        expensesDbContext.SaveChanges();
        expensesDbContext.ChangeTracker.Clear();
    }
    public FakeAPIApplicationRepository(List<ExpenseDbo> expenses): base()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase($"testing_blog-blazor-{Guid.NewGuid()}")
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
    public FakeAPIApplicationRepository(IDateProvider dateProvider) : base()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase($"testing_blog-blazor-3")
            .Options; 
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
    }

    public FakeAPIApplicationRepository(ExpensesDbContext expensesDbContext, IDateProvider dateProvider) : 
        base(expensesDbContext)
    {
        this.expensesDbContext = expensesDbContext;
        expensesDbContext.Database.EnsureCreated();
    }
}