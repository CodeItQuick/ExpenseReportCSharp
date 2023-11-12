
namespace Application.Adapter;

public interface IExistingExpensesRepository
{
    public Domain.ExpenseReport? GetLastExpenseReport();
    public Domain.ExpenseReport? AddAggregate(List<Expense> expenseList, DateTimeOffset? expenseDate);
    public Domain.ExpenseReport? UpdateAggregate(List<Expense> expenses, int expenseReportId);
}