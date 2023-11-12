using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ExistingExpensesRepository
{
    private readonly ExpensesDbContext expensesDbContext;

    public ExistingExpensesRepository()
    {
        var dbContextOptions = new DbContextOptionsBuilder()
            .UseSqlite("Data Source=blog.db")
            .Options;
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
    }

    public ExistingExpensesRepository(ExpensesDbContext expensesDbContext)
    {
        this.expensesDbContext = expensesDbContext;
    }

    public ExpenseReportAggregate? GetLastExpenseReport()
    {
        return expensesDbContext.ExpenseReportAggregates.ToList().LastOrDefault();
    }

    public void ReplaceAllExpenses(List<Expenses> expenseList)
    {
        var expenseReportAggregates = expensesDbContext.ExpenseReportAggregates.ToList();
        expensesDbContext.ExpenseReportAggregates.RemoveRange(expenseReportAggregates);
        expensesDbContext.SaveChanges();
        if (expenseList.Any())
        {
            var expenseReportAggregate = new ExpenseReportAggregate
            {
                Expenses = expenseList
            };
            expensesDbContext.ExpenseReportAggregates.Add(expenseReportAggregate);
            expensesDbContext.SaveChanges();
        }
    }

    public ExpenseReportAggregate? AddAggregate(ExpenseReportAggregate expenseReport)
    {
        var entityEntry = expensesDbContext.ExpenseReportAggregates.Add(expenseReport);
        expensesDbContext.SaveChanges();
        return entityEntry.Entity;
    }
}