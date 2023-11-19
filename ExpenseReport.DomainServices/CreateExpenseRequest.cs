using Domain;

namespace ExpenseReport.ApplicationServices;

public class CreateExpenseRequest
{
    public ExpenseType type { get; set; }
    public int amount { get; set; }
    public int expenseReportId { get; set; }
}