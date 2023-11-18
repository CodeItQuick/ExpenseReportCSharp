using Domain;

namespace Tests;

public class ExpenseListTests
{
    [Fact]
    public void CanCalculateMealExpenses()
    {
        var expenseList = new ExpenseList(
            new List<Expense>(), 1);

        var calculateMealExpenses = expenseList.CalculateMealExpenses();
        
        Assert.Equal(0, calculateMealExpenses);
    }
    [Fact]
    public void CanRetrieveIndividualExpenses()
    {
        var expenseList = new ExpenseList(
            new List<Expense>(), 1);

        var calculateIndividualExpenses = expenseList.RetrieveIndividualExpenses();
        
        Assert.Equal(0, calculateIndividualExpenses.Count);
    }
    [Fact]
    public void CanCalculateTotalExpenses()
    {
        var expenseList = new ExpenseList(
            new List<Expense>(), 1);

        var calculateIndividualExpenses = expenseList.CalculateTotalExpenses();
        
        Assert.Equal(0, calculateIndividualExpenses);
    }
    [Fact]
    public void CanRetrieveExpenseListDate()
    {
        var expenseList = new ExpenseList(
            new List<Expense>(), 1);

        var calculateIndividualExpenses = expenseList.CalculateTotalExpenses();
        
        Assert.Equal(0, calculateIndividualExpenses);
    }
}