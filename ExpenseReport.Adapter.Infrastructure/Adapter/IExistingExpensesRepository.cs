
namespace Application.Adapter;

public interface IExistingExpensesRepository
{
    public Domain.ExpenseReport? GetLastExpenseReport();
    public Domain.ExpenseReport? CreateAggregate(List<Expense> expenseList, DateTimeOffset? expenseDate);
    public Domain.ExpenseReport? UpdateAggregate(List<Expense> expenses, int expenseReportId);
    public List<Domain.ExpenseReport> ListAllExpenseReports();
}