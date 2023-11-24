using Domain;
using Newtonsoft.Json;

namespace WebApplication1.Controllers;

public class ExpenseView
{
    [JsonProperty("mealExpenses")]
    public int MealExpenses { get; set; }
    [JsonProperty("totalExpenses")]
    public int TotalExpenses { get; set; }
    [JsonProperty("expenseDate")]
    public DateTimeOffset ExpenseDate { get; set; }
    [JsonProperty("individualExpenses")]
    public List<ExpenseDto> IndividualExpenses { get; set; }
    [JsonProperty("id")]
    public int Id { get; set; }
    public List<int>? ExpenseReportIds { get; set; }

    public static List<ExpenseDto> CreateTransformedExpenses(Domain.ExpenseReport expenseAdded)
    {
        List<ExpenseDto> transformedExpenses = new List<ExpenseDto>();
        foreach (Expense expenses in expenseAdded.CalculateIndividualExpenses())
        {
            transformedExpenses.Add(new ExpenseDto()
            {
                ExpenseType = expenses.ExpenseTypes().ToString(),
                Amount = expenses.Amount(),
                IsOverExpensedMeal = expenses.IsOverExpensedMeal() ? "OverExpensed" : "",
            });
        }

        return transformedExpenses;
    }
}