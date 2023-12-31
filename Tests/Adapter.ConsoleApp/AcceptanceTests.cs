using Application.Adapter;
using Domain;
using ExpenseReportCSharp.Adapter;

namespace Tests.Adapter.ConsoleApp;

public class AcceptanceTests
{
    [Fact]
    public void EmptyExpenseReportShowsEmptyReceipt()
    {
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("4/5/2023")), 
            new List<ExpenseDbo>(),
            systemOutProvider,
            new FakeExistingRepository(new List<ExpenseDbo>()));

        expensePrinter.PrintExistingReport();

        Assert.Equal(new List<string>()
        {
            "Expenses 4/5/2023 12:00:00 AM -03:00",
            "Meal expenses: 0",
            "Total expenses: 0"
        }, systemOutProvider.Messages());
    }

    [Fact]
    public void OneBreakfastExpenseReportShowsMealExpense()
    {
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        var expenses = new ExpenseDbo() { ExpenseType = ExpenseType.BREAKFAST, Amount = 10};
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("4/5/2023")),
            null,
            systemOutProvider,
            new FakeExistingRepository(new List<ExpenseDbo>()
            {
                expenses,
            }));

        expensePrinter.PrintExistingReport();

        Assert.Equal(systemOutProvider.Messages(), 
            new List<string>()
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
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("4/5/2023")),
            new List<ExpenseDbo>() ,
            systemOutProvider,
            new FakeExistingRepository(new List<ExpenseDbo>()
            {
                new() { ExpenseType = ExpenseType.DINNER, Amount = 10 }
            }));

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
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        var expenses = new ExpenseDbo() { ExpenseType = ExpenseType.CAR_RENTAL, Amount = 10};
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("4/5/2023")),
            null,
            systemOutProvider,
            new FakeExistingRepository(new List<ExpenseDbo>()
            {
                expenses,
            }));

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
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("4/5/2023")),
            new List<ExpenseDbo>(),
            systemOutProvider,
            new FakeExistingRepository(new List<ExpenseDbo>()
            {
                new() { ExpenseType = ExpenseType.DINNER, Amount = 5010 },
            }));

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
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("4/5/2023")),
            null,
            systemOutProvider,
            new FakeExistingRepository(new List<ExpenseDbo>()
            {
                new() { ExpenseType = ExpenseType.BREAKFAST, Amount = 1010, ExpenseReportId = 1}
            }));

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
        FakeSystemOutProvider fakeSystemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("4/5/2023")),
            new List<ExpenseDbo>(),
            fakeSystemOutProvider,
            new FakeExistingRepository(new List<ExpenseDbo>()
            {
                new() { ExpenseType = ExpenseType.BREAKFAST, Amount = 500 },
                new() { ExpenseType = ExpenseType.DINNER, Amount = 5010 },
                new() { ExpenseType = ExpenseType.CAR_RENTAL, Amount = 1010 },
            }));

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