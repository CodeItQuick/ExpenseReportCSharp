using Domain;
using ExpenseReport.ApplicationServices;
using Microsoft.EntityFrameworkCore;
using ExpenseReport = Domain.ExpenseReport;

namespace Application.Adapter;

public class ExistingExpensesRepository : IExistingExpensesRepository
{
    private readonly ExpensesDbContext expensesDbContext;

    public ExistingExpensesRepository()
    {
        var dbContextOptions = new DbContextOptionsBuilder()
            .UseSqlite("Data Source=blog.db")
            .Options; // FIXME: Broken
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
    }

    public ExistingExpensesRepository(ExpensesDbContext expensesDbContext)
    {
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
            var expenseReportAggregate = new ExpenseReportAggregate
            {
                Expenses = expenseList.Select(x =>
                {
                    var tryParse = Enum.TryParse<ExpenseType>(x.expenseType(), out var expenseType);
                    return new Expenses(expenseType, x.Amount());
                }).ToList()
            };
            expensesDbContext.ExpenseReportAggregates.Add(expenseReportAggregate);
            expensesDbContext.SaveChanges();
        }
    }

    // FIXME: Ultra broke this
    public Domain.ExpenseReport? AddAggregate(Domain.ExpenseReport expenseReport)
    {
        var entityEntry = expensesDbContext.ExpenseReportAggregates.Add(new ExpenseReportAggregate()
        {
            Expenses = expenseReport.CalculateIndividualExpenses().Select(x =>
            {
                return new Expenses(ExpenseType.DINNER, 100);
            }).ToList()
        });
        expensesDbContext.SaveChanges();
        return expenseReport;
    }
}