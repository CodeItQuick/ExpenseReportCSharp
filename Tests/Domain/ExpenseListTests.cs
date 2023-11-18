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
    public void CanCalculateOneMealExpense()
    {
        var expenseList = new ExpenseList(
            new List<Expense>() { new Expense(ExpenseType.DINNER, 1000) }, 1);

        var calculateMealExpenses = expenseList.CalculateMealExpenses();
        
        Assert.Equal(1000, calculateMealExpenses);
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
    public void CanRetrieveSingleIndividualExpenses()
    {
        var expenseList = new ExpenseList(
            new List<Expense>() { new Expense(ExpenseType.CAR_RENTAL, 1111) }, 1);

        var calculateIndividualExpenses = expenseList.RetrieveIndividualExpenses();
        
        Assert.Equal(1, calculateIndividualExpenses.Count);
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
    public void CanCalculateATotalExpense()
    {
        var expenseList = new ExpenseList(
            new List<Expense>() { new Expense(ExpenseType.DINNER, 1234) }, 1);

        var calculateIndividualExpenses = expenseList.CalculateTotalExpenses();
        
        Assert.Equal(1234, calculateIndividualExpenses);
    }
}