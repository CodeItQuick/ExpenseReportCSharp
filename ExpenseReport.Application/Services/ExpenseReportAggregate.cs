using Domain;

namespace Application.Services;

public class ExpensesReportAggregate
{
    public int Id { get; set; }
    private readonly List<Expenses> expenses;
    
    public ExpensesReportAggregate(List<Expenses> expenses, int id)
    {
        this.expenses = expenses;
        Id = id;
    }

    public List<Expense> RetrieveExpenseList()
    {
        return expenses
            .Select(x => new Expense(x.ExpenseType, x.Amount))
            .ToList();
    }
}