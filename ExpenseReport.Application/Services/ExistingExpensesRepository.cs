namespace Application.Services;

public class ExistingExpensesRepository
{
    private ExpensesContext expensesContext;

    public ExistingExpensesRepository() {
        expensesContext = new ExpensesContext();
    }

    public List<ExpenseDto> AllExpenses() {
        return expensesContext.Expenses.ToList();
    }

    public void ReplaceAllExpenses(List<ExpenseDto> expenseList) {
        var expenseDtos = expensesContext.Expenses.ToList();
        expensesContext.Expenses.RemoveRange(expenseDtos);
        expensesContext.SaveChanges();
        expensesContext.Expenses.AddRange(expenseList);
        expensesContext.SaveChanges();
    }
}