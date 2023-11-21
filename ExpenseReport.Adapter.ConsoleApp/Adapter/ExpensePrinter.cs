
using Application.Adapter;
using Domain;
using ExpenseReport.ApplicationServices;

namespace ExpenseReportCSharp.Adapter;

public class ExpensePrinter
{
    private readonly IDateProvider dateProvider;
    private SystemOutProvider systemOutProvider;
    private ExpensesService expensesService;

    public ExpensePrinter(
        IDateProvider dateProvider, 
        List<ExpenseDbo> expenseList, 
        SystemOutProvider systemOutProvider,
        IExistingExpensesRepository existingExpensesRepository) {
        this.dateProvider = dateProvider;
        this.systemOutProvider = systemOutProvider;
        expensesService = new ExpensesService(existingExpensesRepository);
    }

    private ExpensePrinter(IDateProvider dateProvider) {
        this.dateProvider = dateProvider;
        systemOutProvider = new SystemOutProvider();
        expensesService = new ExpensesService(new ExistingExpensesRepository());
    }

    public static ExpensePrinter Create() {
        return new ExpensePrinter(new RealDateProvider()); // FIXME: broken
    }

    public void PrintExistingReport() {
        var expenseReport = expensesService.RetrieveExpenseReport(1);

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
            // FIXME: put this method somewhere
            List<string> displayExpenses = new List<string>();
            foreach (Expense expense in expenseReport.CalculateIndividualExpenses()) {
                String label = expense.ExpenseTypes() + "\t" + expense.Amount() + "\t" +
                               (expense.IsOverExpensedMeal() ? "X" : " ");
                displayExpenses.Add(label);
            }
            // FIXME: put this method somewhere
            expenseView = new ExpenseView(
                expenseReport.CalculateMealExpenses(),
                expenseReport.CalculateTotalExpenses(),
                dateProvider.CurrentDate(),
                displayExpenses);
        }
        
        
        systemOutProvider.ServicePrint(expenseView.reportTitle());
        foreach(string expenseMessage in expenseView.displayIndividualExpenses()) {
            systemOutProvider.ServicePrint(expenseMessage);
        }
        systemOutProvider.ServicePrint(expenseView.mealExpenseTotal());
        systemOutProvider.ServicePrint(expenseView.TotalExpenses());
    }
}