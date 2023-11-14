 using Application.Services;
 using Domain;
 using ExpenseReportCSharp.Adapter;

 namespace Tests;

public class DomainTests
{
    // ZOMBIES -> Zero, One, Many, Boundaries, Interface, Exceptions, Solid
    // Unit Tests on Domain
    [Fact]
    public void CanAddAMealExpense()
    {
        // Given (Setup)
        var expenseReport = new Domain.ExpenseReport(new List<Expense>(), DateTimeOffset.Now, 1);
        Expense firstExpense = new Expense(ExpenseType.BREAKFAST, 500);

        // When (Call a method or general unit)
        expenseReport.AddExpense(firstExpense);

        // Then I expect some result
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
}