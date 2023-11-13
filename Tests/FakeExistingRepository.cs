using Application.Adapter;
using ExpenseReport.ApplicationServices;
using Microsoft.EntityFrameworkCore;
using ExpenseReport = Domain.ExpenseReport;

namespace Tests;

public class FakeExistingRepository : ExistingExpensesRepository
{
    public FakeExistingRepository(): base(new RealDateProvider())
    {
        var dbContextOptions = new DbContextOptionsBuilder()
            .UseInMemoryDatabase($"testing_blog-1")
            .Options; 
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
    }
    public FakeExistingRepository(List<Expense> expenses): base(new RealDateProvider())
    {
        var dbContextOptions = new DbContextOptionsBuilder()
            .UseInMemoryDatabase($"testing_blog-2")
            .Options; 
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
        var expenseList = expensesDbContext.Expenses.ToList();
        expensesDbContext.Expenses.RemoveRange(expenseList);
        expensesDbContext.SaveChanges();
        expensesDbContext.Expenses.AddRange(expenses);
        expensesDbContext.SaveChanges();
        expensesDbContext.ChangeTracker.Clear();
    }
    public FakeExistingRepository(IDateProvider dateProvider) : base(dateProvider)
    {
        var dbContextOptions = new DbContextOptionsBuilder()
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