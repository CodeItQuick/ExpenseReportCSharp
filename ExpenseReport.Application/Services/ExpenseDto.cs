using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.Services;

public class ExpenseDto
{
    [Key]
    public int Id { get; set; }
    public ExpenseType ExpenseType { get; set; }
    public int Amount { get; set; }

    public ExpenseDto(ExpenseType expenseType, int amount)
    {
        ExpenseType = expenseType;
        Amount = amount;
    }
}