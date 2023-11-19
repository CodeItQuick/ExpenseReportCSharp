using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.Adapter;

public sealed class ExpenseReport
{
    [Key]
    public int Id { get; private set; }

    public List<ExpenseDbo>? Expenses { get; set; }
    
    public DateTimeOffset ExpenseReportDate { get; set; }
}