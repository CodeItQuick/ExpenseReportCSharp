
using Application.Services;

namespace ExpenseReportCSharp.Adapter;

public class ExpensePrinter
{
    
    private SystemOutProvider systemOutProvider;
    private ExpensesService expensesService;

    public ExpensePrinter(DateProvider dateProvider, List<Expenses> expenseList, SystemOutProvider systemOutProvider) {
        this.systemOutProvider = systemOutProvider;
        this.expensesService = new ExpensesService(dateProvider, expenseList);
    }

    public ExpensePrinter(DateProvider dateProvider) {
        this.systemOutProvider = new SystemOutProvider();
        this.expensesService = new ExpensesService(dateProvider);
    }

    public static ExpensePrinter Create() {
        return new ExpensePrinter(new RealDateProvider());
    }

    public void PrintExistingReport() {
        ExpenseView expenseView = expensesService.viewExpenses();

        this.systemOutProvider.ServicePrint(expenseView.reportTitle());
        foreach(string expenseMessage in expenseView.displayIndividualExpenses()) {
            this.systemOutProvider.ServicePrint(expenseMessage);
        }
        this.systemOutProvider.ServicePrint(expenseView.mealExpenseTotal());
        this.systemOutProvider.ServicePrint(expenseView.TotalExpenses());
    }
}