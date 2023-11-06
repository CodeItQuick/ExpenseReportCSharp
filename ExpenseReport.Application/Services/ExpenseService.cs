using Domain;

namespace Application.Services;

public class ExpensesService
{
    private ExistingExpensesRepository expenseRepository;
    private readonly DateProvider dateProvider;

    // Used by Tests
    public ExpensesService(
        DateProvider dateProvider, 
        List<Expenses> expenseList, 
        ExistingExpensesRepository existingExpensesRepository) : this(dateProvider) {
        expenseRepository = existingExpensesRepository;
        expenseRepository.ReplaceAllExpenses(expenseList);
    }

    // Used By Production Code, One Smoke Test
    public ExpensesService(DateProvider? dateProvider) {
        this.dateProvider = dateProvider ?? new RealDateProvider();
        expenseRepository = new ExistingExpensesRepository();
    }

    public ExpenseReport RetrieveExpenseReport() {
        var expensesReportAggregate = expenseRepository.GetLastExpenseReport();

        ExpenseReport expenseReport = new ExpenseReport(
            expensesReportAggregate?.RetrieveExpenseList() ?? new List<Expense>(),
            this.dateProvider);
        
        return expenseReport;
    }
}