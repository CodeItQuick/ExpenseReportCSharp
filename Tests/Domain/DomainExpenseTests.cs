using Domain;

namespace Tests;

public class DomainExpenseTests
{
    [Fact]
    public void ExpenseCanCalculateMealExpense()
    {
        Expense expense = new Expense(ExpenseType.BREAKFAST, 500);

        Assert.Equal(500, expense.CalculateMealExpenses());
    }
    [Fact]
    public void ExpenseDoesNotCalculateWhenCarRental()
    {
        Expense expense = new Expense(ExpenseType.CAR_RENTAL, 500);

        Assert.Equal(0, expense.CalculateMealExpenses());
    }
    [Fact]
    public void DinnerExpenseCalculatesIfOverExpensed()
    {
        Expense expense = new Expense(ExpenseType.DINNER, 5001);

        Assert.Equal(true, expense.IsOverExpensedMeal());
    }
    [Fact]
    public void DinnerExpenseCalculatesIfNotOverExpensed()
    {
        Expense expense = new Expense(ExpenseType.DINNER, 4950);

        Assert.Equal(false, expense.IsOverExpensedMeal());
    }
    [Fact]
    public void BreakfastExpenseCalculatesIfOverExpensed()
    {
        Expense expense = new Expense(ExpenseType.BREAKFAST, 1001);

        Assert.Equal(true, expense.IsOverExpensedMeal());
    }
    [Fact]
    public void BreakfastExpenseCalculatesIfNotOverExpensed()
    {
        Expense expense = new Expense(ExpenseType.BREAKFAST, 1000);

        Assert.Equal(false, expense.IsOverExpensedMeal());
    }
}