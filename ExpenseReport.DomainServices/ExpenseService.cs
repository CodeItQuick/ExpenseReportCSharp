using Application.Adapter;
using Domain;

namespace ExpenseReport.ApplicationServices;

public class ExpensesService : IExpenseService
{
    private readonly IExistingExpensesRepository expenseRepository;

    public ExpensesService(IExistingExpensesRepository existingExpensesRepository) {
        expenseRepository = existingExpensesRepository;
    }

    public Domain.ExpenseReport CreateExpense(Domain.Expense expense, DateTimeOffset expenseDate)
    {
        var expenseList = new List<Expense>()
            {
                expense
            };
        var addExpenseToReport = expenseRepository.CreateAggregate(expenseList, expenseDate);
        if (addExpenseToReport == null)
        {
            throw new Exception("expense report failed to save");
        }
        return addExpenseToReport;
    }

    public Domain.ExpenseReport? RetrieveExpenseReport(int id) {
        return expenseRepository.RetrieveById(id);
    }

    public List<Domain.ExpenseReport> ListAllExpenseReports() {
        return expenseRepository.ListAllExpenseReports();
    }

    public Domain.ExpenseReport CreateExpenseReport(DateTimeOffset expenseReportDate)
    {
        
        var addExpenseToReport = expenseRepository.CreateAggregate(new List<Expense>(), expenseReportDate);
        if (addExpenseToReport == null)
        {
            throw new Exception("expense report failed to save");
        }
        return addExpenseToReport;
    }

    public Domain.ExpenseReport AddExpenseToExpenseReport(int expenseReportId, Expense expense)
    {
        var expenses = new List<Expense>()
            {
                new(expense.ExpenseTypes(), expense.Amount())
            };
        var addExpenseToReport = expenseRepository.UpdateAggregate(expenses, expenseReportId);
        if (addExpenseToReport == null)
        {
            throw new Exception("expense report failed to save");
        }
        return addExpenseToReport;
    }
}