namespace Domain;

// Aggregate
public class ExpenseReport
{
    private readonly ExpenseList expenses;
    private DateTimeOffset ExpenseReportDate { get; }
    public int Id { get; }


    public ExpenseReport(List<Expense>? expenses, DateTimeOffset expenseReportDate, int id)
    {
        this.expenses = new ExpenseList(expenses ?? new List<Expense>(), id);
        ExpenseReportDate = expenseReportDate;
        Id = id;
    }

    public DateTimeOffset RetrieveDate()
    {
        return ExpenseReportDate;
    }

    public List<String> CalculateIndividualExpenses() {
        List<string> displayExpenses = new List<string>();
        foreach (Expense expense in expenses.RetrieveIndividualExpenses()) {
            String label = expense.ExpenseTypes() + "\t" + expense.Amount() + "\t" +
                           (expense.IsOverExpensedMeal() ? "X" : " ");
            displayExpenses.Add(label);
        }
        return displayExpenses;
    }

    public int CalculateTotalExpenses() {
        return expenses.CalculateTotalExpenses();
    }

    public int CalculateMealExpenses() {
        return expenses.CalculateMealExpenses();
    }

    public void AddExpense(Expense firstExpense)
    {
        expenses.Add(firstExpense);
    }
}