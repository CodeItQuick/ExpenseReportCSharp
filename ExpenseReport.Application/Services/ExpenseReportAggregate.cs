using Domain;

namespace Application.Services;

public class ExpensesReportAggregate
{
    private readonly List<Expenses> expenses;
    public int Id { get; set; }
    
    public ExpensesReportAggregate(List<Expenses> expenses)
    {
        this.expenses = expenses;
    }

    public List<Expense> RetrieveExpenseList()
    {
        return expenses
            .Select(x => new Expense(x.ExpenseType, x.Amount))
            .ToList();
    }
}