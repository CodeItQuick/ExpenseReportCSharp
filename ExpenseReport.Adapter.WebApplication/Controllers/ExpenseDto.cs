namespace WebApplication1.Controllers;

public class ExpenseDto
{
    public string ExpenseType { get; init; }
    public int Amount { get; init; }
    public string IsOverExpensedMeal { get; init; }
}