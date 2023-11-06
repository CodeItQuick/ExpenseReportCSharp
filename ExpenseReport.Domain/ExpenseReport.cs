using Application.Services;

namespace Domain;

public class ExpenseReport
{
    private List<Expense> expenses;
    private readonly DateProvider dateProvider;

    public ExpenseReport(List<Expense> expenses, DateProvider dateProvider)
    {
        this.expenses = expenses;
        this.dateProvider = dateProvider;
    }

    public DateTimeOffset RetrieveDate()
    {
        return dateProvider.CurrentDate();
    }

    public List<String> CalculateIndividualExpenses() {
        List<string> displayExpenses = new List<string>();
        foreach (Expense expense in this.expenses) {
            String label = expense.expenseType() + "\t" + expense.Amount() + "\t" + expense.isOverExpensedMeal();
            displayExpenses.Add(label);
        }
        return displayExpenses;
    }

    public int CalculateTotalExpenses() {
        int total = 0;
        foreach (Expense expense in expenses) {
            total += expense.Amount();
        }
        return total;
    }

    public int CalculateMealExpenses() {
        int mealExpenses = 0;
        foreach (Expense expense in expenses) {
            mealExpenses += expense.calculateMealExpenses();
        }
        return mealExpenses;
    }

    public void AddExpense(Expense firstExpense)
    {
        expenses.Add(firstExpense);
    }
}