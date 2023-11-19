namespace ExpenseReport.ApplicationServices;

public interface IExpenseService
{
    public Domain.ExpenseReport? RetrieveExpenseReport(int id);
    public List<Domain.ExpenseReport> ListAllExpenseReports(); 
    public Domain.ExpenseReport CreateExpenseReport(DateTimeOffset expenseReportDate);
    public Domain.ExpenseReport AddExpenseToExpenseReport(
        int expenseReportId,
        List<CreateExpenseRequest> createExpenseRequest);
}