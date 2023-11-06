
using Application.Services;
using Domain;

namespace ExpenseReportCSharp.Adapter;

public class ExpensePrinter
{
    
    private SystemOutProvider systemOutProvider;
    private ExpensesService expensesService;

    public ExpensePrinter(DateProvider dateProvider, List<Expenses> expenseList, SystemOutProvider systemOutProvider) {
        this.systemOutProvider = systemOutProvider;
        expensesService = new ExpensesService(dateProvider, expenseList, new ExistingExpensesRepository());
    }

    public ExpensePrinter(DateProvider dateProvider) {
        systemOutProvider = new SystemOutProvider();
        expensesService = new ExpensesService(dateProvider);
    }

    public static ExpensePrinter Create() {
        return new ExpensePrinter(new RealDateProvider());
    }

    public void PrintExistingReport() {
        ExpenseReport expenseReport = expensesService.ViewExpenses();

        var expenseView = new ExpenseView(
            expenseReport.CalculateMealExpenses(),
            expenseReport.CalculateTotalExpenses(),
            expenseReport.RetrieveDate(),
            expenseReport.CalculateIndividualExpenses());
        
        
        systemOutProvider.ServicePrint(expenseView.reportTitle());
        foreach(string expenseMessage in expenseView.displayIndividualExpenses()) {
            systemOutProvider.ServicePrint(expenseMessage);
        }
        systemOutProvider.ServicePrint(expenseView.mealExpenseTotal());
        systemOutProvider.ServicePrint(expenseView.TotalExpenses());
    }
}