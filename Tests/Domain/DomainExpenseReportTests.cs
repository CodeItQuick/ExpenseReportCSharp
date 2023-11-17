 using Application.Services;
 using Domain;
 using ExpenseReportCSharp.Adapter;

 namespace Tests;

public class DomainExpenseReportTests
{
    [Fact]
    public void CanAddAMealExpense()
    {
        var expenseReport = new Domain.ExpenseReport(new List<Expense>(), DateTimeOffset.Now, 1);
        Expense firstExpense = new Expense(ExpenseType.BREAKFAST, 500);

        expenseReport.AddExpense(firstExpense);

        Assert.Equal(500, expenseReport.CalculateMealExpenses());
    }
    [Fact]
    public void CanAddASecondExpense()
    {
        var expenseReport = new Domain.ExpenseReport(new List<Expense>(), DateTimeOffset.Now, 1);
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
        var expenseReport = new Domain.ExpenseReport(expenseList, DateTimeOffset.Now, 1);
        
        Assert.Equal(new(), expenseReport.CalculateIndividualExpenses());
    }
    [Fact]
    public void CanCalculateSingleIndividualExpenses()
    {
        var expenseList = new List<Expense>()
        {
            new(ExpenseType.BREAKFAST, 500)
        };
        var expenseReport = new Domain.ExpenseReport(expenseList, DateTimeOffset.Now, 1);
        
        Assert.Equal(new() { "BREAKFAST\t500\t " }, expenseReport.CalculateIndividualExpenses());
    }
    [Fact]
    public void CanCalculateMultipleIndividualExpenses()
    {
        var expenseList = new List<Expense>()
        {
            new(ExpenseType.BREAKFAST, 500),
            new(ExpenseType.DINNER, 500),
        };
        var expenseReport = new Domain.ExpenseReport(expenseList, DateTimeOffset.Now, 1);
        
        Assert.Equal(new() { "BREAKFAST\t500\t ", "DINNER\t500\t " }, expenseReport.CalculateIndividualExpenses());
    }
    [Fact]
    public void CanCalculateOverExpensedIndividualExpenses()
    {
        var expenseList = new List<Expense>()
        {
            new(ExpenseType.BREAKFAST, 1001),
            new(ExpenseType.DINNER, 5001),
        };
        var expenseReport = new Domain.ExpenseReport(expenseList, DateTimeOffset.Now, 1);
        
        Assert.Equal(new() { "BREAKFAST\t1001\tX", "DINNER\t5001\tX" }, expenseReport.CalculateIndividualExpenses());
    }
    [Fact]
    public void CanCalculateSingleMealExpense()
    {
        var expenseList = new List<Expense>()
        {
            new(ExpenseType.BREAKFAST, 300),
        };
        var expenseReport = new Domain.ExpenseReport(expenseList, DateTimeOffset.Now, 1);
        
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
        var expenseReport = new Domain.ExpenseReport(expenseList, DateTimeOffset.Now, 1);
        
        Assert.Equal(700, expenseReport.CalculateMealExpenses());
    }
    [Fact]
    public void DoesNotCalculateCarRental()
    {
        var expenseList = new List<Expense>()
        {
            new(ExpenseType.CAR_RENTAL, 300),
        };
        var expenseReport = new Domain.ExpenseReport(expenseList, DateTimeOffset.Now, 1);
        
        Assert.Equal(0, expenseReport.CalculateMealExpenses());
    }
}