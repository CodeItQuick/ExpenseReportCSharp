 using Application.Services;
 using Domain;
 using ExpenseReportCSharp.Adapter;

namespace Tests;

public class AcceptanceTests
{
    [Fact]
    public void EmptyExpenseReportShowsEmptyReceipt()
    {
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("2023-04-05")), new List<ExpenseDto>(),
            systemOutProvider);

        expensePrinter.PrintExistingReport();

        Assert.Equal(systemOutProvider.Messages(), new List<string>()
        {
            "Expenses 4/5/2023 12:00:00 AM -03:00",
            "Meal expenses: 0",
            "Total expenses: 0"
        });
    }

    [Fact]
    public void OneBreakfastExpenseReportShowsMealExpense()
    {
        ExpenseDto expense = new ExpenseDto(ExpenseType.BREAKFAST, 10);
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("2023-04-05")),
            new List<ExpenseDto>() { expense },
            systemOutProvider);

        expensePrinter.PrintExistingReport();

        Assert.Equal(systemOutProvider.Messages(), new List<string>()
        {
            "Expenses 4/5/2023 12:00:00 AM -03:00",
            "BREAKFAST	10	 ",
            "Meal expenses: 10",
            "Total expenses: 10"
        });
    }

    [Fact]
    public void OneDinnerExpenseReportShowsMealExpense()
    {
        ExpenseDto expense = new ExpenseDto(ExpenseType.DINNER, 10);
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("2023-04-05")),
            new List<ExpenseDto>() { expense },
            systemOutProvider);

        expensePrinter.PrintExistingReport();

        Assert.Equal(systemOutProvider.Messages(), new List<string>()
        {
            "Expenses 4/5/2023 12:00:00 AM -03:00",
            "DINNER\t10\t ",
            "Meal expenses: 10",
            "Total expenses: 10"
        });
    }

    [Fact]
    public void OneCarRentalExpenseReportShowsMealExpense()
    {
        ExpenseDto expense = new ExpenseDto(ExpenseType.CAR_RENTAL, 10);
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("2023-04-05")),
            new List<ExpenseDto>() { expense },
            systemOutProvider);

        expensePrinter.PrintExistingReport();

        Assert.Equal(systemOutProvider.Messages(), new List<string>()
        {
            "Expenses 4/5/2023 12:00:00 AM -03:00",
            "CAR_RENTAL\t10\t ",
            "Meal expenses: 0",
            "Total expenses: 10"
        });
    }

    [Fact]
    public void OneDinnerExpenseOverMaximumReportShowsMealExpenseAndMarker()
    {
        ExpenseDto expense = new ExpenseDto(ExpenseType.DINNER, 5010);
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("2023-04-05")),
            new List<ExpenseDto>() { expense },
            systemOutProvider);

        expensePrinter.PrintExistingReport();

        Assert.Equal(systemOutProvider.Messages(), new List<string>()
        {
            "Expenses 4/5/2023 12:00:00 AM -03:00",
            "DINNER\t5010\tX",
            "Meal expenses: 5010",
            "Total expenses: 5010"
        });
    }

    [Fact]
    public void OneBreakfastExpenseOverMaximumReportShowsMealExpenseAndMarker()
    {
        ExpenseDto expense = new ExpenseDto(ExpenseType.BREAKFAST, 1010);
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("2023-04-05")),
            new List<ExpenseDto>() { expense },
            systemOutProvider);

        expensePrinter.PrintExistingReport();

        Assert.Equal(systemOutProvider.Messages(), new List<string>()
        {
            "Expenses 4/5/2023 12:00:00 AM -03:00",
            "BREAKFAST\t1010\tX",
            "Meal expenses: 1010",
            "Total expenses: 1010"
        });
    }

    [Fact]
    public void MultipleMealsReportShowsAllExpenses()
    {
        ExpenseDto firstExpense = new ExpenseDto(ExpenseType.BREAKFAST, 500);
        ExpenseDto secondExpense = new ExpenseDto(ExpenseType.DINNER, 5010);
        ExpenseDto thirdExpense = new ExpenseDto(ExpenseType.CAR_RENTAL, 1010);
        FakeSystemOutProvider fakeSystemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("2023-04-05")),
            new List<ExpenseDto>() { firstExpense, secondExpense, thirdExpense },
            fakeSystemOutProvider);

        expensePrinter.PrintExistingReport();

        Assert.Equal( new List<string>()
        {
            "Expenses 4/5/2023 12:00:00 AM -03:00",
            "BREAKFAST	500	 ",
            "DINNER	5010	X",
            "CAR_RENTAL	1010	 ",
            "Meal expenses: 5510",
            "Total expenses: 6520"
        }, fakeSystemOutProvider.Messages());
    }
}