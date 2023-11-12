 using Application.Adapter;
 using Application.Services;
 using Domain;
 using ExpenseReport.ApplicationServices;
 using ExpenseReportCSharp.Adapter;
 using ExpenseReport = Domain.ExpenseReport;

 namespace Tests;

public class AcceptanceTests
{
    [Fact]
    public void EmptyExpenseReportShowsEmptyReceipt()
    {
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("4/5/2023")), 
            new List<Expenses>(),
            systemOutProvider,
            new FakeExistingRepository());

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
        Expenses expense = new Expenses(ExpenseType.BREAKFAST, 10);
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("4/5/2023")),
            new List<Expenses>() { expense },
            systemOutProvider,
            new FakeExistingRepository(new List<Expense>()
            {
                new(expense.ExpenseType, expense.Amount),
            }));

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
            new FakeDateProvider(DateTimeOffset.Parse("4/5/2023")),
            new List<Expenses>() { expense },
            systemOutProvider,
            new FakeExistingRepository(new List<Expense>()
            {
                new(expense.ExpenseType, expense.Amount),
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
        Expenses expense = new Expenses(ExpenseType.CAR_RENTAL, 10);
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("4/5/2023")),
            new List<Expenses>() { expense },
            systemOutProvider,
            new FakeExistingRepository(new List<Expense>()
            {
                new(expense.ExpenseType, expense.Amount),
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
        Expenses expense = new Expenses(ExpenseType.DINNER, 5010);
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("4/5/2023")),
            new List<Expenses>() { expense },
            systemOutProvider,
            new FakeExistingRepository(new List<Expense>()
            {
                new(expense.ExpenseType, expense.Amount),
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
        Expenses expense = new Expenses(ExpenseType.BREAKFAST, 1010);
        FakeSystemOutProvider systemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("4/5/2023")),
            new List<Expenses>() { expense },
            systemOutProvider,
            new FakeExistingRepository(new List<Expense>()
            {
                new(expense.ExpenseType, expense.Amount),
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
        Expenses firstExpense = new Expenses(ExpenseType.BREAKFAST, 500);
        Expenses secondExpense = new Expenses(ExpenseType.DINNER, 5010);
        Expenses thirdExpense = new Expenses(ExpenseType.CAR_RENTAL, 1010);
        FakeSystemOutProvider fakeSystemOutProvider = new FakeSystemOutProvider();
        ExpensePrinter expensePrinter = new ExpensePrinter(
            new FakeDateProvider(DateTimeOffset.Parse("4/5/2023")),
            new List<Expenses>() { firstExpense, secondExpense, thirdExpense },
            fakeSystemOutProvider,
            new FakeExistingRepository(new List<Expense>()
            {
                new(firstExpense.ExpenseType, firstExpense.Amount),
                new(secondExpense.ExpenseType, secondExpense.Amount),
                new(thirdExpense.ExpenseType, thirdExpense.Amount),
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

 public class FakeExistingRepository : IExistingExpensesRepository
 {
     private readonly List<Expense>? expensesList;

     public FakeExistingRepository()
     {
     }

     public FakeExistingRepository(List<Expense>? expensesList)
     {
         this.expensesList = expensesList;
     }

     public Domain.ExpenseReport? GetLastExpenseReport()
     {
         if (expensesList != null && expensesList.Any())
         {
             return new Domain.ExpenseReport(this.expensesList);
         }
         return null;
     }

     public void ReplaceAllExpenses(List<Expense> expenseList)
     {
         
     }

     public Domain.ExpenseReport? AddAggregate(Domain.ExpenseReport expenseReport)
     {
         return null;
     }
 }