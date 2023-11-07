using Domain;

namespace Application.Services;

public class ExpenseView
{
    public int MealExpenses;
    public int TotalExpenses;
    public DateTimeOffset ExpenseDate;
    public List<String>? IndividualExpenses;
}