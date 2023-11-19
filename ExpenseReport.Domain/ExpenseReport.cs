namespace Domain;

// Aggregate - Needs to return primitives/value objects *only* because these are going to be used by adapters that
// do not want to have to do unnecessary translations from domain -> primitive for the "external world"
// to use them. Within the domain use Entities where possible (primitive obsession code smell)

// Downsides of returning entities
// Map properties -> primitives/value objects somehow (Huge - this can be 1000s of lines of code)
// These mapped properties have to be done for EVERY adapter if we do it this way, rather than just a single time in the aggregate.
// This results in having to change a bunch of adapter code for changes in the domain.
// What we get
// Behaviour inside the entities. But do we want this? Can just treat the code as immutable if we return primitives/value objects
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

    // Interesting Use-Case
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