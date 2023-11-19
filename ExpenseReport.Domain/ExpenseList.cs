namespace Domain;

// Entity (Has *some* logic in it)
public class ExpenseList
{
    private readonly List<Expense> expenses;
    public int Id { get; }
    
    public ExpenseList(List<Expense>? expenses, int id)
    {
        this.expenses = expenses ?? new List<Expense>();
        Id = id;
    }
    
    // Interesting Use-Case
    public List<Expense> RetrieveIndividualExpenses() {
        return expenses;
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
            mealExpenses += expense.CalculateMealExpenses();
        }
        return mealExpenses;
    }

    public void Add(Expense firstExpense)
    {
        expenses.Add(firstExpense);
    }
}