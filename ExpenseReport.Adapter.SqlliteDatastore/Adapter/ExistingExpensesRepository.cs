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
            .Options; 
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
            expenseReportAggregates?.RetrieveExpenseList() ?? new List<Expense>(), 
            expenseReportAggregates?.ExpenseReportDate ?? DateTimeOffset.Now, expenseReportAggregates.Id);
    }

    public Domain.ExpenseReport? AddAggregate(List<Expense> expenseReport, DateTimeOffset? expenseDate)
    {
        var expenseReportAggregate = new ExpenseReportAggregate(expenseReport, expenseDate ?? realDateProvider.CurrentDate());
        var entityEntry = expensesDbContext.ExpenseReportAggregates.Add(
            expenseReportAggregate);
        expensesDbContext.SaveChanges();
        expensesDbContext.ChangeTracker.Clear();
        return entityEntry.Entity?.RetrieveExpenseReport();
    }

    public Domain.ExpenseReport? UpdateAggregate(List<Expense> expenseReport, int expenseReportId)
    {
        var expenseReportAggregate = new ExpenseReportAggregate(expenseReport, realDateProvider.CurrentDate(), expenseReportId);
        var entityEntry = expensesDbContext.ExpenseReportAggregates.Update(
            expenseReportAggregate);
        expensesDbContext.SaveChanges();
        return entityEntry.Entity?.RetrieveExpenseReport();
    }
}