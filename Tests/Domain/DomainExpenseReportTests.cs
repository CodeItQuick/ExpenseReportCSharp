using Domain;

namespace Tests.Domain;

public class DomainExpenseReportTests
{
    [Fact]
    public void CanAddMealExpense()
    {
        var expenseReport = new global::Domain.ExpenseReport(new List<Expense>(), DateTimeOffset.Now, 1, "abcd-1234");
        Expense firstExpense = new Expense(ExpenseType.BREAKFAST, 500);

        expenseReport.AddExpense(firstExpense);

        Assert.Equal(500, expenseReport.CalculateMealExpenses());
    }
    [Fact]
    public void CanAddSecondExpense()
    {
        var expenseReport = new global::Domain.ExpenseReport(new List<Expense>(), DateTimeOffset.Now, 1, "abcd-1234");
        Expense firstExpense = new Expense(ExpenseType.BREAKFAST, 500);
        expenseReport.AddExpense(firstExpense);

        Expense secondExpense = new Expense(ExpenseType.DINNER, 5010);
        expenseReport.AddExpense(secondExpense);

        Assert.Equal(5510, expenseReport.CalculateMealExpenses());
    }
    [Fact]
    public void CanCalculateZeroIndividualExpenses()
    {
        var expenseList = new List<Expense>();
        var expenseReport = new global::Domain.ExpenseReport(expenseList, DateTimeOffset.Now, 1, "abcd-1234");
        
        Assert.Equal(new(), expenseReport.CalculateIndividualExpenses());
    }
    [Fact]
    public void CanCalculateSingleMealExpense()
    {
        var expenseList = new List<Expense>()
        {
            new(ExpenseType.BREAKFAST, 300),
        };
        var expenseReport = new global::Domain.ExpenseReport(expenseList, DateTimeOffset.Now, 1, "abcd-1234");
        
        Assert.Equal(300, expenseReport.CalculateMealExpenses());
    }
    [Fact]
    public void CanCalculateMultipleMealExpense()
    {
        var expenseList = new List<Expense>()
        {
            new(ExpenseType.BREAKFAST, 300),
            new(ExpenseType.DINNER, 400),
        };
        var expenseReport = new global::Domain.ExpenseReport(expenseList, DateTimeOffset.Now, 1, "abcd-1234");
        
        Assert.Equal(700, expenseReport.CalculateMealExpenses());
    }
    [Fact]
    public void DoesNotCalculateCarRental()
    {
        var expenseList = new List<Expense>()
        {
            new(ExpenseType.CAR_RENTAL, 300),
        };
        var expenseReport = new global::Domain.ExpenseReport(expenseList, DateTimeOffset.Now, 1, "abcd-1234");
        
        Assert.Equal(0, expenseReport.CalculateMealExpenses());
    }
}