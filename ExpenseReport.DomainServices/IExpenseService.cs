using Domain;

namespace ExpenseReport.ApplicationServices;

public interface IExpenseService
{
    public Domain.ExpenseReport CreateExpense(int expenseCost, ExpenseType expense, DateTimeOffset expenseDate);
    public Domain.ExpenseReport? RetrieveExpenseReport();
    public Domain.ExpenseReport CreateExpenseReport(DateTimeOffset expenseReportDate);
    public Domain.ExpenseReport CreateExpense(int expenseCost, ExpenseType expenseType, int expenseReportId);

}