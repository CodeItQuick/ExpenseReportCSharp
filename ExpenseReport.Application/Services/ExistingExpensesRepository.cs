namespace Application.Services;

public class ExistingExpensesRepository
{
    private readonly ExpensesContext expensesContext = new();

    public List<Expenses> GetAllExpenses() {
        return expensesContext.Expenses.ToList();
    }
    public ExpenseReportAggregate? GetExpenseReport()
    {
        return expensesContext.ExpenseReportAggregates.ToList().LastOrDefault();
    }

    public void ReplaceAllExpenses(List<Expenses> expenseList)
    {
        var expenses = expensesContext.Expenses.ToList();
        expensesContext.Expenses.RemoveRange(expenses);
        expensesContext.SaveChanges();

        var expenseReportAggregate = new ExpenseReportAggregate();
        if (expenseList.Any())
        {
            expenseReportAggregate.Expenses = expenseList;
        }
        expensesContext.ExpenseReportAggregates.Add(expenseReportAggregate);
        expensesContext.SaveChanges();
    }
}