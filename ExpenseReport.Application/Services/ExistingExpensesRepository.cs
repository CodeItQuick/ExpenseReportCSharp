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

    public List<Expenses> GetAllExpenses() {
        return expensesContext.Expenses.ToList();
    }
    public ExpenseReportAggregate? GetLastExpenseReport()
    {
        return expensesContext.ExpenseReportAggregates.ToList().LastOrDefault();
    }

    public void ReplaceAllExpenses(List<Expenses> expenseList)
    {
        var expenseReportAggregate = new ExpenseReportAggregate();
        if (expenseList.Any())
        {
            expenseReportAggregate.Expenses = expenseList;
        }
        expensesContext.ExpenseReportAggregates.Add(expenseReportAggregate);
        expensesContext.SaveChanges();
    }
}