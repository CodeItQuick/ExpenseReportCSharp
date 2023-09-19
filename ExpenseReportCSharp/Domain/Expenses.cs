namespace ExpenseReportCSharp.Domain;

public class Expenses
{
    private List<Expense> expenses;

    public Expenses(List<Expense> expenses) {
        this.expenses = expenses;
    }

    public List<String> calculateIndividualExpenses() {
        List<string> displayExpenses = new List<string>();
        foreach (Expense expense in this.expenses) {
            String label = expense.expenseType() + "\t" + expense.Amount() + "\t" + expense.isOverExpensedMeal();
            displayExpenses.Add(label);
        }
        return displayExpenses;
    }

    public int calculateTotalExpenses() {
        int total = 0;
        foreach (Expense expense in this.expenses) {
            total += expense.Amount();
        }
        return total;
    }

    public int calculateMealExpenses() {
        int mealExpenses = 0;
        foreach (Expense expense in this.expenses) {
            mealExpenses += expense.calculateMealExpenses();
        }
        return mealExpenses;
    }
}