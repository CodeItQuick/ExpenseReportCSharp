using Newtonsoft.Json;

namespace Application.Services;

public class ExpenseView
{
    [JsonProperty("mealExpenses")]
    public int MealExpenses { get; set; }
    [JsonProperty("totalExpenses")]
    public int TotalExpenses { get; set; }
    [JsonProperty("expenseDate")]
    public DateTimeOffset ExpenseDate { get; set; }
    [JsonProperty("individualExpenses")]
    public List<String>? IndividualExpenses { get; set; }
    [JsonProperty("id")]
    public int Id { get; set; }
}