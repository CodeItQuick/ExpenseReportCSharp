
using Domain;
using ExpenseReport.ApplicationServices;

namespace Application.Adapter;

public interface IExistingExpensesRepository
{
    public Domain.ExpenseReport? RetrieveById(int reportId, string employeeId);
    public Domain.ExpenseReport? CreateAggregate(DateTimeOffset? expenseDate,
        List<CreateExpenseRequest> createExpenseRequests, string employeeId);
    public Domain.ExpenseReport? UpdateAggregate(List<CreateExpenseRequest> createExpenseRequests);
    public List<Domain.ExpenseReport> ListAllExpenseReports(string userId);
}