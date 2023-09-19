namespace ExpenseReportCSharp.Adapter;

public class ExpenseView
{
    private int mealExpenses;
    private int totalExpense;
    private String expenseDate;
    private List<String> individualExpenses;

    public ExpenseView(int mealExpenses, int total, String expenseDate, List<String> individualExpensesTwo) {
        this.mealExpenses = mealExpenses;
        this.totalExpense = total;
        this.expenseDate = expenseDate;
        this.individualExpenses = individualExpensesTwo;
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
        return "Total expenses: " + this.totalExpense;
    }
    
}