namespace ExpenseReportCSharp.Services;

public class ExpenseDto
{
    public ExpenseType ExpenseType { get; set; }
    public int Amount { get; set; }

    public ExpenseDto(ExpenseType expenseType, int amount)
    {
        ExpenseType = expenseType;
        Amount = amount;
    }
}