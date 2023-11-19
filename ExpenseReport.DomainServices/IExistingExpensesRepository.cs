
using Domain;
using ExpenseReport.ApplicationServices;

namespace Application.Adapter;

public interface IExistingExpensesRepository
{
    public Domain.ExpenseReport? RetrieveById(int reportId);
    public Domain.ExpenseReport? CreateAggregate(DateTimeOffset? expenseDate,
        List<CreateExpenseRequest> createExpenseRequests);
    public Domain.ExpenseReport? UpdateAggregate(List<CreateExpenseRequest> createExpenseRequests);
    public List<Domain.ExpenseReport> ListAllExpenseReports();
}