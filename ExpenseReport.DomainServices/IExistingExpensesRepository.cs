using Domain;

namespace ExpenseReport.ApplicationServices;

public interface IExistingExpensesRepository
{
    public Domain.ExpenseReport? GetLastExpenseReport();
    public Domain.ExpenseReport? AddAggregate(List<Expense> expenseReport, DateTimeOffset? expenseDate);
}