using Domain;
using ExpenseReport.ApplicationServices;
using Microsoft.EntityFrameworkCore;

namespace Application.Adapter;

public class ExistingExpensesRepository : IExistingExpensesRepository
{
    protected ExpensesDbContext expensesDbContext;

    public ExistingExpensesRepository(IDateProvider dateProvider)
    {
        var dbContextOptions = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseSqlite("Data Source=blog.db")
            .Options; 
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
    }

    public ExistingExpensesRepository(ExpensesDbContext expensesDbContext, IDateProvider dateProvider)
    {
        this.expensesDbContext = expensesDbContext;
    }

    public Domain.ExpenseReport? GetLastExpenseReport()
    {
        var reportAggregates = expensesDbContext.ExpenseReportAggregates
            .Include("Expenses")
            .ToList();
        var expenseReportAggregates = reportAggregates
            .LastOrDefault();
        if (expenseReportAggregates == null || !reportAggregates.Any())
        {
            return null;
        }
        return new Domain.ExpenseReport(
            expenseReportAggregates.Expenses?.Select(x => new Domain.Expense(x.ExpenseType, x.Amount)).ToList(), 
            expenseReportAggregates?.ExpenseReportDate ?? DateTimeOffset.Now, 
            expenseReportAggregates.Id);
    }

    public Domain.ExpenseReport? CreateAggregate(List<Expense> expenseList, DateTimeOffset? expenseDate)
    {
        var expenseReportAggregate = new ExpenseReportAggregate()
        {
            Expenses = expenseList,
            ExpenseReportDate = expenseDate ?? DateTimeOffset.Now
        };
        var entityEntry = expensesDbContext.ExpenseReportAggregates.Add(
            expenseReportAggregate);
        expensesDbContext.SaveChanges();
        expensesDbContext.ChangeTracker.Clear();
        return new Domain.ExpenseReport(
            expenseList.Select(x => new Domain.Expense(x.ExpenseType, x.Amount)).ToList(), 
            entityEntry.Entity?.ExpenseReportDate ?? DateTimeOffset.Now, 
            entityEntry.Entity?.Id ?? 0);
    }

    public Domain.ExpenseReport? UpdateAggregate(List<Expense> expenses, int expenseReportId)
    {
        var reportAggregate = expensesDbContext
            .ExpenseReportAggregates
            .Include("Expenses")
            .FirstOrDefault(x => x.Id == expenseReportId);
        if (reportAggregate?.Expenses != null && reportAggregate.Expenses.Any())
        {
            reportAggregate.Expenses.AddRange(expenses);
        }
        else
        {
            reportAggregate.Expenses = expenses;
        }
        expensesDbContext.ExpenseReportAggregates.Update(reportAggregate);
        expensesDbContext.SaveChanges();
        return new Domain.ExpenseReport(
            reportAggregate?.Expenses.Select(x => new Domain.Expense(x.ExpenseType, x.Amount)).ToList(), 
            reportAggregate?.ExpenseReportDate ?? DateTimeOffset.Now, 
            reportAggregate.Id);
    }
}