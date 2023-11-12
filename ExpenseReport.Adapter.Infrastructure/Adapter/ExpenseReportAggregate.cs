using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.Adapter;

public sealed class ExpenseReportAggregate
{
    [Key]
    public int Id { get; private set; }

    public List<Expenses>? Expenses { get; private set; }
    
    public DateTimeOffset ExpenseReportDate { get; private set; }

    public ExpenseReportAggregate()
    {
    }

    public ExpenseReportAggregate(List<Expense>? expenses, DateTimeOffset expenseReportDate)
    {
        Expenses = expenses?.Select(x =>
        {
            var tryParse = Enum.TryParse<ExpenseType>(x.expenseType(), out var expenseType);
            return new Expenses(expenseType, x.Amount());
        }).ToList() ?? new List<Expenses>();
        ExpenseReportDate = expenseReportDate;
    }
    public ExpenseReportAggregate(List<Expense>? expenses, DateTimeOffset expenseReportDate, int expenseReportId)
    {
        Expenses = expenses?.Select(x =>
        {
            var tryParse = Enum.TryParse<ExpenseType>(x.expenseType(), out var expenseType);
            return new Expenses(expenseType, x.Amount());
        }).ToList() ?? new List<Expenses>();
        ExpenseReportDate = expenseReportDate;
        Id = expenseReportId;
    }

    public Domain.ExpenseReport RetrieveExpenseReport()
    {
        return new Domain.ExpenseReport(Expenses?
            .Select(x => new Expense(x.ExpenseType, x.Amount))
            .ToList(), ExpenseReportDate, Id);
    }

    public List<Expense> RetrieveExpenseList()
    {
        return Expenses
            ?.Select(x => new Expense(x.ExpenseType, x.Amount))
            .ToList() ?? new List<Expense>();
    }

    public void AddExpense(List<Expense> expenses)
    {
        if (Expenses != null && Expenses.Any())
        {
            Expenses.AddRange(expenses.Select(x =>
            {
                var parseSuccess = Enum.TryParse<ExpenseType>(x.expenseType(), out var expenseType);
                return new Expenses(expenseType, x.Amount());
            }).ToList());
        }
        else
        {
            Expenses = expenses.Select(x =>
            {
                var parseSuccess = Enum.TryParse<ExpenseType>(x.expenseType(), out var expenseType);
                return new Expenses(expenseType, x.Amount());
            }).ToList();
        }
    }
}