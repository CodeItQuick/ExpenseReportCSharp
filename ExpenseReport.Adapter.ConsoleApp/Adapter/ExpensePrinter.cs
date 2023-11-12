
using Application.Adapter;
using ExpenseReport.ApplicationServices;

namespace ExpenseReportCSharp.Adapter;

public class ExpensePrinter
{
    private readonly IDateProvider dateProvider;
    private SystemOutProvider systemOutProvider;
    private ExpensesService expensesService;

    public ExpensePrinter(
        IDateProvider dateProvider, 
        List<Expenses> expenseList, 
        SystemOutProvider systemOutProvider,
        IExistingExpensesRepository existingExpensesRepository) {
        this.dateProvider = dateProvider;
        this.systemOutProvider = systemOutProvider;
        expensesService = new ExpensesService(
            dateProvider, 
            existingExpensesRepository);
    }

    private ExpensePrinter(IDateProvider dateProvider) {
        this.dateProvider = dateProvider;
        systemOutProvider = new SystemOutProvider();
        expensesService = new ExpensesService(
            new RealDateProvider(), 
            new ExistingExpensesRepository(new RealDateProvider()));
    }

    public static ExpensePrinter Create() {
        return new ExpensePrinter(new RealDateProvider()); // FIXME: broken
    }

    public void PrintExistingReport() {
        var expenseReport = expensesService.RetrieveExpenseReport();

        ExpenseView expenseView;
        if (expenseReport == null)
        {
            expenseView = new ExpenseView(
                0,
                0,
                dateProvider.CurrentDate(),
                new List<string>());
        }
        else
        {
            expenseView = new ExpenseView(
                expenseReport.CalculateMealExpenses(),
                expenseReport.CalculateTotalExpenses(),
                dateProvider.CurrentDate(),
                expenseReport.CalculateIndividualExpenses());
        }
        
        
        systemOutProvider.ServicePrint(expenseView.reportTitle());
        foreach(string expenseMessage in expenseView.displayIndividualExpenses()) {
            systemOutProvider.ServicePrint(expenseMessage);
        }
        systemOutProvider.ServicePrint(expenseView.mealExpenseTotal());
        systemOutProvider.ServicePrint(expenseView.TotalExpenses());
    }
}