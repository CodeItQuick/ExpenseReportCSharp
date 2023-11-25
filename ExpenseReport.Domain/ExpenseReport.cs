namespace Domain;

public class ExpenseReport
{
    private readonly List<Expense> expenseList;
    private DateTimeOffset ExpenseReportDate { get; }
    public int Id { get; }
    public string EmployeeId { get; set; }

    public bool Approved { get; set; }

    public ExpenseReport(
        List<Expense>? expenses, 
        DateTimeOffset expenseReportDate, 
        int id, 
        string expenseReportEmployeeId, 
        bool isApproved)
    {
        expenseList = expenses ?? new List<Expense>();
        ExpenseReportDate = expenseReportDate;
        Id = id;
        EmployeeId = expenseReportEmployeeId;
        Approved = isApproved;
    }

    public DateTimeOffset RetrieveDate()
    {
        return ExpenseReportDate;
    }

    // Interesting Use-Case

    public List<Expense> CalculateIndividualExpenses() {
        return expenseList;
    }

    public int CalculateTotalExpenses()
    {
        int total = 0;
        foreach (Expense expense in expenseList) {
            total += expense.Amount();
        }
        return total;
    }

    public int CalculateMealExpenses()
    {
        int mealExpenses = 0;
        foreach (Expense expense in expenseList) {
            mealExpenses += expense.CalculateMealExpenses();
        }
        return mealExpenses;
    }

    public void AddExpense(Expense firstExpense)
    {
        expenseList.Add(firstExpense);
    }

    public bool IsApproved()
    {
        return Approved;
    }
}