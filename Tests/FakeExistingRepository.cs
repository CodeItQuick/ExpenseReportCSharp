using Application.Adapter;
using ExpenseReport = Domain.ExpenseReport;

namespace Tests;

public class FakeExistingRepository : IExistingExpensesRepository
{
    private List<Expense>? expensesList;

    public FakeExistingRepository()
    {
    }

    public FakeExistingRepository(List<Expense>? expensesList)
    {
        this.expensesList = expensesList;
    }

    public Domain.ExpenseReport? GetLastExpenseReport()
    {
        if (expensesList != null && expensesList.Any())
        {
            var expenses = this.expensesList
                .Select(x => new Domain.Expense(x.ExpenseType, x.Amount))
                .ToList();
            return new Domain.ExpenseReport(expenses, DateTimeOffset.Now, 1);
        }
        return new Domain.ExpenseReport(new List<Domain.Expense>(), DateTimeOffset.Now, 1);
    }

    public void ReplaceAllExpenses(List<Expense> expenseList)
    {
         
    }

    public Domain.ExpenseReport? AddAggregate(List<Expense> expenseList, DateTimeOffset? expenseDate)
    {
        expensesList = expenseList;
        
        if (expensesList != null)
            return new Domain.ExpenseReport(
                expensesList.Select(x => new Domain.Expense(x.ExpenseType, x.Amount)).ToList(),
                expenseDate ?? DateTimeOffset.Now, 1);
        return new Domain.ExpenseReport(
            new List<Domain.Expense>(),
            expenseDate ?? DateTimeOffset.Now, 1);
    }

    public Domain.ExpenseReport? UpdateAggregate(List<Expense> expenses, int expenseReportId)
    {
        if (expensesList == null || !expensesList.Any())
        {
            expensesList = expenses;
        }
        else
        {
            expensesList.AddRange(expenses);
        }
        if (expensesList != null)
            return new Domain.ExpenseReport(
                expensesList.Select(x => new Domain.Expense(x.ExpenseType, x.Amount)).ToList(),
                DateTimeOffset.Now, 1);
        return new Domain.ExpenseReport(
            new List<Domain.Expense>(),
            DateTimeOffset.Now, 1);
    }
}