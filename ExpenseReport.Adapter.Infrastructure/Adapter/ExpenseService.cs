using Domain;
using ExpenseReport.ApplicationServices;

namespace Application.Adapter;

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
                new() { ExpenseType = expense.ExpenseTypes(), Amount = expense.Amount()}
            };
        var addExpenseToReport = expenseRepository.CreateAggregate(expenseList, expenseDate);
        if (addExpenseToReport == null)
        {
            throw new Exception("expense report failed to save");
        }
        return addExpenseToReport;
    }

    public Domain.ExpenseReport? RetrieveExpenseReport() {
        return expenseRepository.GetLastExpenseReport();
    }
    public List<Domain.ExpenseReport> RetrieveAllExpenseReports() {
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

    public Domain.ExpenseReport CreateExpense(int expenseReportId, Domain.Expense expense)
    {
        
        var expenses = new List<Expense>()
            {
                new()
                {
                    ExpenseType = expense.ExpenseTypes(), 
                    Amount = expense.Amount(), 
                    ExpenseReportAggregateId = expenseReportId
                } 
            };
        var addExpenseToReport = expenseRepository.UpdateAggregate(expenses, expenseReportId);
        if (addExpenseToReport == null)
        {
            throw new Exception("expense report failed to save");
        }
        return addExpenseToReport;
    }
}