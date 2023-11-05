 using Application.Services;
 using Domain;
 using ExpenseReportCSharp.Adapter;
 using Expenses = Application.Services.Expenses;

 namespace Tests;

public class AcceptanceTests
{
    [Fact]
    public void EmptyExpenseReportShowsEmptyReceipt()
    {
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("2023-04-05")), 
            new List<Expenses>(),
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
        Expenses expense = new Expenses(ExpenseType.BREAKFAST, 10);
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("2023-04-05")),
            new List<Expenses>() { expense },
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
        Expenses expense = new Expenses(ExpenseType.DINNER, 10);
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("2023-04-05")),
            new List<Expenses>() { expense },
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
        Expenses expense = new Expenses(ExpenseType.CAR_RENTAL, 10);
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("2023-04-05")),
            new List<Expenses>() { expense },
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
        Expenses expense = new Expenses(ExpenseType.DINNER, 5010);
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("2023-04-05")),
            new List<Expenses>() { expense },
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
        Expenses expense = new Expenses(ExpenseType.BREAKFAST, 1010);
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("2023-04-05")),
            new List<Expenses>() { expense },
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
        Expenses firstExpense = new Expenses(ExpenseType.BREAKFAST, 500);
        Expenses secondExpense = new Expenses(ExpenseType.DINNER, 5010);
        Expenses thirdExpense = new Expenses(ExpenseType.CAR_RENTAL, 1010);
        FakeSystemOutProvider fakeSystemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("2023-04-05")),
            new List<Expenses>() { firstExpense, secondExpense, thirdExpense },
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
    // Unit Tests on Domain
    [Fact]
    public void CanAddAMealExpense()
    {
        var expenseReport = new ExpenseReport(new List<Expense>());

        Expense firstExpense = new Expense(ExpenseType.BREAKFAST, 500);
        expenseReport.AddExpense(firstExpense);

        Assert.Equal(500, expenseReport.CalculateMealExpenses());
    }
    [Fact]
    public void CanAddASecondExpense()
    {
        var expenseReport = new ExpenseReport(new List<Expense>());
        Expense firstExpense = new Expense(ExpenseType.BREAKFAST, 500);
        expenseReport.AddExpense(firstExpense);

        Expense secondExpense = new Expense(ExpenseType.DINNER, 5010);
        expenseReport.AddExpense(secondExpense);

        Assert.Equal(5510, expenseReport.CalculateMealExpenses());
    }
}