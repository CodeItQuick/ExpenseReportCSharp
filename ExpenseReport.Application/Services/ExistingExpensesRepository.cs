namespace Application.Services;

public class ExistingExpensesRepository
{
    private ExpensesContext expensesContext;

    public ExistingExpensesRepository() {
        expensesContext = new ExpensesContext();
    }

    public List<Expenses> GetAllExpenses() {
        return expensesContext.Expenses.ToList();
    }
    public ExpensesReportAggregate GetExpenseReport(int id)
    {
        var expensesList = expensesContext.Expenses.ToList();
        var expenseList = expensesList;
        var expensesReportAggregate = new ExpensesReportAggregate(expenseList);
        return expensesReportAggregate;
    }

    public void ReplaceAllExpenses(List<Expenses> expenseList) {
        var expenseDtos = expensesContext.Expenses.ToList();
        expensesContext.Expenses.RemoveRange(expenseDtos);
        expensesContext.SaveChanges();
        expensesContext.Expenses.AddRange(expenseList);
        expensesContext.SaveChanges();
    }
}