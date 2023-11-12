using Application.Services;
using Domain;
using ExpenseReport.ApplicationServices;
using Microsoft.EntityFrameworkCore;
using ExpenseReport = Domain.ExpenseReport;

namespace Application.Adapter;

public class ExistingExpensesRepository : IExistingExpensesRepository
{
    private readonly ExpensesDbContext expensesDbContext;
    private readonly IDateProvider realDateProvider;

    public ExistingExpensesRepository(IDateProvider dateProvider)
    {
        var dbContextOptions = new DbContextOptionsBuilder()
            .UseSqlite("Data Source=blog.db")
            .Options; // FIXME: Broken
        realDateProvider = dateProvider;
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
    }

    public ExistingExpensesRepository(ExpensesDbContext expensesDbContext, IDateProvider dateProvider)
    {
        realDateProvider = dateProvider;
        this.expensesDbContext = expensesDbContext;
    }

    public Domain.ExpenseReport? GetLastExpenseReport()
    {
        var reportAggregates = expensesDbContext.ExpenseReportAggregates
            .ToList();
        var expenseReportAggregates = reportAggregates
            .LastOrDefault();
        if (expenseReportAggregates == null || !reportAggregates.Any())
        {
            return null;
        }
        return new Domain.ExpenseReport(
            expenseReportAggregates?.RetrieveExpenseList() ?? new List<Expense>());
    }

    public void ReplaceAllExpenses(List<Expense> expenseList)
    {
        var expenseReportAggregates = expensesDbContext.ExpenseReportAggregates.ToList();
        expensesDbContext.ExpenseReportAggregates.RemoveRange(expenseReportAggregates);
        expensesDbContext.SaveChanges();
        if (expenseList.Any())
        {
            var expenseReportAggregate = new ExpenseReportAggregate(expenseList, realDateProvider.CurrentDate());
            expensesDbContext.ExpenseReportAggregates.Add(expenseReportAggregate);
            expensesDbContext.SaveChanges();
        }
    }

    public Domain.ExpenseReport? AddAggregate(List<Expense> expenseReport, DateTimeOffset? expenseDate)
    {
        var expenseReportAggregate = new ExpenseReportAggregate(expenseReport, expenseDate ?? realDateProvider.CurrentDate());
        var entityEntry = expensesDbContext.ExpenseReportAggregates.Add(
            expenseReportAggregate);
        expensesDbContext.SaveChanges();
        return entityEntry.Entity?.RetrieveExpenseReport();
    }
}