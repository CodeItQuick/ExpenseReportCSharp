using Domain;

namespace Application.Services;

public class ExpenseView
{
    private int mealExpenses;
    private int totalExpenses;
    private DateTimeOffset expenseDate;
    private List<String> individualExpenses;

    public ExpenseView(ExpenseReport expenseReport) {
        this.mealExpenses = expenseReport.CalculateMealExpenses();
        this.totalExpenses = expenseReport.CalculateTotalExpenses();
        this.expenseDate = expenseReport.RetrieveDate();
        this.individualExpenses = expenseReport.CalculateIndividualExpenses();
    }

    public List<string> DisplayIndividualExpenses() {
        List<string> individualExpenses = new List<string>();
        foreach (string individualExpense in this.individualExpenses) {
            string message = individualExpense;
            individualExpenses.Add(message);
        }
        return individualExpenses;
    }

    public string ReportTitle() {
        return "Expenses " + this.expenseDate;
    }

    public string MealExpenseTotal() {
        return "Meal expenses: " + this.mealExpenses;
    }

    public string TotalExpenses() {
        return "Total expenses: " + this.totalExpenses;
    }
    
}