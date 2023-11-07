namespace Application.Services;

public class ExpenseView
{
    public int MealExpenses { get; set; }
    public int TotalExpenses { get; set; }
    public DateTimeOffset ExpenseDate { get; set; }
    public List<String>? IndividualExpenses { get; set; }
}