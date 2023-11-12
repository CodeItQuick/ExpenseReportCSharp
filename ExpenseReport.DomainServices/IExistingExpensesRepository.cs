using Domain;

namespace ExpenseReport.ApplicationServices;

public interface IExistingExpensesRepository
{
    public Domain.ExpenseReport? GetLastExpenseReport();

    public void ReplaceAllExpenses(List<Expense> expenseList);

    // FIXME: Ultra broke this
    public Domain.ExpenseReport? AddAggregate(Domain.ExpenseReport expenseReport);
}