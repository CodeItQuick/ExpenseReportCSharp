using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.Adapter;

public sealed class ExpenseReportAggregate
{
    [Key]
    public int Id { get; set; }

    public List<Expenses>? Expenses { get; set; }
    
    public DateTimeOffset ExpenseReportDate { get; set; }

    public List<Expense> RetrieveExpenseList()
    {
        return Expenses?
            .Select(x => new Expense(x.ExpenseType, x.Amount))
            .ToList();
    }
}