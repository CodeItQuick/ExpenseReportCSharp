using Domain;

namespace ExpenseReport.ApplicationServices;

public interface IExpenseService
{
    public Domain.ExpenseReport CreateExpense(Expense expense, DateTimeOffset expenseDate); // this should not exist
    public Domain.ExpenseReport? RetrieveExpenseReport(int id);
    public List<Domain.ExpenseReport> ListAllExpenseReports(); 
    public Domain.ExpenseReport CreateExpenseReport(DateTimeOffset expenseReportDate);
    public Domain.ExpenseReport AddExpenseToExpenseReport(int expenseReportId, Expense expense);

}