using ExpenseReportCSharp.Adapter;
using ExpenseReportCSharp.Domain;

namespace ExpenseReportCSharp.Services;

public class ExpensesService
{
    private ExistingExpensesRepository expenseRepository;
    private DateProvider dateProvider;

    public ExpensesService(DateProvider dateProvider, List<ExpenseDto> expenseList) : this(dateProvider) {
        this.expenseRepository.ReplaceAllExpenses(expenseList);
    }

    public ExpensesService(DateProvider? dateProvider) {
        this.dateProvider = dateProvider ?? new RealDateProvider();
        this.expenseRepository = new ExistingExpensesRepository();
    }

    public ExpenseView viewExpenses() {
        List<ExpenseDto> allExpenses = this.expenseRepository.AllExpenses();
        List<Expense> transformToDomain = allExpenses
            .Select(x => new Expense(x.ExpenseType, x.Amount))
            .ToList();

        Expenses expenses = new Expenses(transformToDomain);
        int mealExpenses = expenses.calculateMealExpenses();
        int total = expenses.calculateTotalExpenses();
        List<String> individualExpenses = expenses.calculateIndividualExpenses();

        return new ExpenseView(mealExpenses, total, this.dateProvider.CurrentDate().ToString(), individualExpenses);
    }
}