using Domain;
using ExpenseReport.ApplicationServices;

namespace Application.Adapter;

public class ExpensesService : IExpenseService
{
    private readonly IExistingExpensesRepository expenseRepository;
    private readonly IDateProvider dateProvider;

    // Used By Production Code, One Smoke Test
    public ExpensesService(IDateProvider dateProvider, IExistingExpensesRepository existingExpensesRepository) {
        this.dateProvider = dateProvider;
        expenseRepository = existingExpensesRepository;  //new ExistingExpensesRepository();
    }

    public Domain.ExpenseReport CreateExpense(Expense expense, DateTimeOffset expenseDate)
    {
        var expenseList = 
            new List<Expense>()
            {
                expense
            };
        var addExpenseToReport = expenseRepository.AddAggregate(expenseList, expenseDate);
        if (addExpenseToReport == null)
        {
            throw new Exception("expense report failed to save");
        }
        return addExpenseToReport;
    }

    public Domain.ExpenseReport? RetrieveExpenseReport() {
        return expenseRepository.GetLastExpenseReport();
    }

    public Domain.ExpenseReport CreateExpenseReport(DateTimeOffset expenseReportDate)
    {
        
        var addExpenseToReport = expenseRepository.AddAggregate(new List<Expense>(), expenseReportDate);
        if (addExpenseToReport == null)
        {
            throw new Exception("expense report failed to save");
        }
        return addExpenseToReport;
    }

    public Domain.ExpenseReport CreateExpense(int expenseReportId, Expense expense)
    {
        
        var expenseReport = 
            new List<Expense>()
            {
                expense
            };
        var addExpenseToReport = expenseRepository.UpdateAggregate(expenseReport, expenseReportId);
        if (addExpenseToReport == null)
        {
            throw new Exception("expense report failed to save");
        }
        return addExpenseToReport;
    }
}