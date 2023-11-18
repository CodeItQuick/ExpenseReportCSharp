using Newtonsoft.Json;

namespace ExpenseReport.Adapter.WebAPI.Controllers;

public class ExpenseApiView
{
    [JsonProperty("mealExpenses")]
    public int MealExpenses { get; set; }
    [JsonProperty("totalExpenses")]
    public int TotalExpenses { get; set; }
    [JsonProperty("expenseDate")]
    public DateTimeOffset ExpenseDate { get; set; }
    [JsonProperty("individualExpenses")]
    public List<string>? IndividualExpenses { get; set; }
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("expenseReportIds")]
    public List<int>? ExpenseReportIds { get; set; }
}