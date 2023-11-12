using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain;

namespace Application.Adapter;

public class Expense
{
    [Key]
    public int Id { get; set; }
    public ExpenseType ExpenseType { get; set; }
    public int Amount { get; set; }

    [ForeignKey("ExpenseReportAggregateId")]
    public int ExpenseReportAggregateId { get; set; }
}