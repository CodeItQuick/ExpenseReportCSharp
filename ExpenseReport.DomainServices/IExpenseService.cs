namespace ExpenseReport.ApplicationServices;

public interface IExpenseService
{
    public Domain.ExpenseReport? RetrieveExpenseReport(int id, string employeeId);
    public List<Domain.ExpenseReport> ListAllExpenseReports(string userId); 
    public Domain.ExpenseReport CreateExpenseReport(DateTimeOffset expenseReportDate, string employeeId);
    public Domain.ExpenseReport AddExpenseToExpenseReport(
        int expenseReportId,
        List<CreateExpenseRequest> createExpenseRequest);
}