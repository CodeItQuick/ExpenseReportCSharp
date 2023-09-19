namespace ExpenseReportCSharp.Services;

public class ExistingExpensesRepository
{
    private ExpensesDatabase expensesDatabase;

    public ExistingExpensesRepository() {
        this.expensesDatabase = new ExpensesDatabase();
    }

    public List<ExpenseDto> AllExpenses() {
        return expensesDatabase.expenses;
    }

    public void ReplaceAllExpenses(List<ExpenseDto> expenseList) {
        this.expensesDatabase.expenses = expenseList;
    }
}