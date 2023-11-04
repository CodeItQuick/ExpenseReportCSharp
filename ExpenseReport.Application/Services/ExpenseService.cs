using Domain;

namespace Application.Services;

public class ExpensesService
{
    private readonly ExistingExpensesRepository expenseRepository;
    private readonly DateProvider dateProvider;

    public ExpensesService(DateProvider dateProvider, List<Expenses> expenseList) : this(dateProvider) {
        expenseRepository.ReplaceAllExpenses(expenseList);
    }

    public ExpensesService(DateProvider? dateProvider) {
        this.dateProvider = dateProvider ?? new RealDateProvider();
        expenseRepository = new ExistingExpensesRepository();
    }

    public ExpenseView viewExpenses() {
        var expensesReportAggregate = expenseRepository.GetExpenseReport(1);

        Domain.Expenses expenses = new Domain.Expenses(expensesReportAggregate.RetrieveExpenseList());
        int mealExpenses = expenses.calculateMealExpenses();
        int total = expenses.calculateTotalExpenses();
        List<String> individualExpenses = expenses.calculateIndividualExpenses();

        return new ExpenseView(mealExpenses, total, dateProvider.CurrentDate().ToString(), individualExpenses);
    }
}