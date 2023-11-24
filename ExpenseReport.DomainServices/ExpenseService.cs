using Application.Adapter;
using Domain;

namespace ExpenseReport.ApplicationServices;

public class ExpensesService : IExpenseService
{
    private readonly IExistingExpensesRepository expenseRepository;

    public ExpensesService(IExistingExpensesRepository existingExpensesRepository) {
        expenseRepository = existingExpensesRepository;
    }

    public Domain.ExpenseReport? RetrieveExpenseReport(int id) {
        return expenseRepository.RetrieveById(id);
    }

    public List<Domain.ExpenseReport> ListAllExpenseReports() {
        return expenseRepository.ListAllExpenseReports();
    }

    public Domain.ExpenseReport CreateExpenseReport(DateTimeOffset expenseReportDate)
    {
        
        var addExpenseToReport = expenseRepository.CreateAggregate(
            expenseReportDate, new List<CreateExpenseRequest>());
        if (addExpenseToReport == null)
        {
            throw new Exception("expense report failed to save");
        }
        return addExpenseToReport;
    }

    public Domain.ExpenseReport AddExpenseToExpenseReport(
        int expenseReportId,
        List<CreateExpenseRequest> createExpenseRequest)
    {
        var expenses = new List<Expense>()
            {
                new(createExpenseRequest.First().type, createExpenseRequest.First().amount)
            };
        var addExpenseToReport = expenseRepository.UpdateAggregate(createExpenseRequest);
        if (addExpenseToReport == null)
        {
            throw new Exception("expense report failed to save");
        }
        return addExpenseToReport;
    }
}