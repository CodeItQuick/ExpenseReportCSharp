
using Application.Services;
using Domain;

namespace ExpenseReport.ApplicationServices;

public class ExpensesService
{
    private IExistingExpensesRepository expenseRepository;
    private readonly IDateProvider dateProvider;

    // Used By Production Code, One Smoke Test
    public ExpensesService(IDateProvider dateProvider, IExistingExpensesRepository existingExpensesRepository) {
        this.dateProvider = dateProvider;
        expenseRepository = existingExpensesRepository;  //new ExistingExpensesRepository();
    }

    public Domain.ExpenseReport? RetrieveExpenseReport() {
        return expenseRepository.GetLastExpenseReport();
    }

    public Domain.ExpenseReport CreateExpense(int expenseCost, ExpenseType expense, DateTimeOffset expenseDate)
    {
        var expenseReport = 
            new List<Expense>()
            {
                new(expense, expenseCost)
            };
        var addExpenseToReport = expenseRepository.AddAggregate(expenseReport, expenseDate);
        if (addExpenseToReport == null)
        {
            throw new Exception("expense report failed to save");
        }
        return addExpenseToReport;
    }

    public Domain.ExpenseReport CreateNewExpenseReport(DateTimeOffset expenseReportDate)
    {
        
        var addExpenseToReport = expenseRepository.AddAggregate(new List<Expense>(), expenseReportDate);
        if (addExpenseToReport == null)
        {
            throw new Exception("expense report failed to save");
        }
        return addExpenseToReport;
    }
}