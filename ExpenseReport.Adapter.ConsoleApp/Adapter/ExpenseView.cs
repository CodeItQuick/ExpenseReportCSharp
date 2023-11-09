namespace ExpenseReportCSharp.Adapter;

public class ExpenseView
{
    private int mealExpenses;
    private int totalExpenses;
    private DateTimeOffset expenseDate;
    private List<String> individualExpenses;

    public ExpenseView(int mealExpenses, int totalExpenses, DateTimeOffset expenseDate, List<String> individualExpenses) {
        this.mealExpenses = mealExpenses;
        this.totalExpenses = totalExpenses;
        this.expenseDate = expenseDate;
        this.individualExpenses = individualExpenses;
    }

    public List<string> displayIndividualExpenses() {
        List<string> individualExpenses = new List<string>();
        foreach (string individualExpense in this.individualExpenses) {
            string message = individualExpense;
            individualExpenses.Add(message);
        }
        return individualExpenses;
    }

    public string reportTitle() {
        return "Expenses " + this.expenseDate;
    }

    public string mealExpenseTotal() {
        return "Meal expenses: " + this.mealExpenses;
    }

    public string TotalExpenses() {
        return "Total expenses: " + this.totalExpenses;
    }
    
}