using Domain;

namespace Application.Services;

public class ExpensesService
{
    private ExistingExpensesRepository expenseRepository;
    private readonly DateProvider dateProvider;

    public ExpensesService(DateProvider dateProvider, List<Expenses> expenseList) : this(dateProvider) {
        expenseRepository = new ExistingExpensesRepository();
        expenseRepository.ReplaceAllExpenses(expenseList);
    }

    public ExpensesService(DateProvider? dateProvider) {
        this.dateProvider = dateProvider ?? new RealDateProvider();
        expenseRepository = new ExistingExpensesRepository();
    }

    public ExpenseView ViewExpenses() {
        var expensesReportAggregate = expenseRepository.GetExpenseReport();

        ExpenseReport expenseReport = new ExpenseReport(expensesReportAggregate?.RetrieveExpenseList() ?? new List<Expense>());
        int mealExpenses = expenseReport.CalculateMealExpenses();
        int total = expenseReport.CalculateTotalExpenses();
        List<String> individualExpenses = expenseReport.CalculateIndividualExpenses();

        return new ExpenseView(mealExpenses, total, dateProvider.CurrentDate().ToString(), individualExpenses);
    }
}