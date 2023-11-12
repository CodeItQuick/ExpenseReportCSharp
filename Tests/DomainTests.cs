 using Application.Services;
 using Domain;
 using ExpenseReportCSharp.Adapter;
 using Expenses = Application.Adapter.Expenses;

 namespace Tests;

public class DomainTests
{
    // Unit Tests on Domain
    [Fact]
    public void CanAddAMealExpense()
    {
        var expenseReport = new Domain.ExpenseReport(new List<Expense>());

        Expense firstExpense = new Expense(ExpenseType.BREAKFAST, 500);
        expenseReport.AddExpense(firstExpense);

        Assert.Equal(500, expenseReport.CalculateMealExpenses());
    }
    [Fact]
    public void CanAddASecondExpense()
    {
        var expenseReport = new Domain.ExpenseReport(new List<Expense>());
        Expense firstExpense = new Expense(ExpenseType.BREAKFAST, 500);
        expenseReport.AddExpense(firstExpense);

        Expense secondExpense = new Expense(ExpenseType.DINNER, 5010);
        expenseReport.AddExpense(secondExpense);

        Assert.Equal(5510, expenseReport.CalculateMealExpenses());
    }
}