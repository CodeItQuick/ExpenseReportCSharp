using Domain;

namespace ExpenseReport.ApplicationServices;

public interface IExpenseService
{
    public Domain.ExpenseReport CreateExpense(Expense expense, DateTimeOffset expenseDate);
    public Domain.ExpenseReport? RetrieveExpenseReport(int id);
    public List<Domain.ExpenseReport> ListAllExpenseReports(); 
    public Domain.ExpenseReport CreateExpenseReport(DateTimeOffset expenseReportDate);
    public Domain.ExpenseReport CreateExpense(int expenseReportId, Expense expense);

}