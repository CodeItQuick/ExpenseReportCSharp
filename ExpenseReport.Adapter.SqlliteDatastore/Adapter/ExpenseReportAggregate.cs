using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    public Domain.ExpenseReport RetrieveExpenseReport()
    {
        return new Domain.ExpenseReport(Expenses?
            .Select(x => new Expense(x.ExpenseType, x.Amount))
            .ToList());
    }

    public List<Expense> RetrieveExpenseList()
    {
        return Expenses?
            .Select(x => new Expense(x.ExpenseType, x.Amount))
            .ToList();
    }
}