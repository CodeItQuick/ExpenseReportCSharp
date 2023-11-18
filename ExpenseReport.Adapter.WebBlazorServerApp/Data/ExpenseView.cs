
namespace ExpenseReport.Adapter.WebBlazorServerApp.Data;

public class ExpenseView
{
    public int MealExpenses { get; set; }
    public int TotalExpenses { get; set; }
    public DateTimeOffset ExpenseDate { get; set; }
    public List<string>? IndividualExpenses { get; set; }
    public int Id { get; set; }
    public List<int>? ExpenseReportIds { get; set; }
}