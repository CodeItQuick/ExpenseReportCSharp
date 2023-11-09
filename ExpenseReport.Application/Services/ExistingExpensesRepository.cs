using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ExistingExpensesRepository
{
    private readonly ExpensesContext expensesContext;

    public ExistingExpensesRepository()
    {
        var DbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "blogging.db");
        var dbContextOptions = new DbContextOptionsBuilder()
            .UseSqlite($"Data Source={DbPath}")
            .Options;
        expensesContext = new ExpensesContext(dbContextOptions);
    }

    public ExistingExpensesRepository(ExpensesContext expensesContext)
    {
        this.expensesContext = expensesContext;
    }

    public ExpenseReportAggregate? GetLastExpenseReport()
    {
        return expensesContext.ExpenseReportAggregates.ToList().LastOrDefault();
    }

    public void ReplaceAllExpenses(List<Expenses> expenseList)
    {
        var expenseReportAggregates = expensesContext.ExpenseReportAggregates.ToList();
        expensesContext.ExpenseReportAggregates.RemoveRange(expenseReportAggregates);
        expensesContext.SaveChanges();
        if (expenseList.Any())
        {
            var expenseReportAggregate = new ExpenseReportAggregate
            {
                Expenses = expenseList
            };
            expensesContext.ExpenseReportAggregates.Add(expenseReportAggregate);
            expensesContext.SaveChanges();
        }
    }

    public ExpenseReportAggregate? AddAggregate(ExpenseReportAggregate expenseReport)
    {
        var entityEntry = expensesContext.ExpenseReportAggregates.Add(expenseReport);
        expensesContext.SaveChanges();
        return entityEntry.Entity;
    }
}