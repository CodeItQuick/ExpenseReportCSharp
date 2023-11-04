using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.Services;

public sealed class ExpensesReportAggregate
{
    [Key]
    public int Id { get; set; }

    public List<Expenses> Expenses { get; set; }

    public ExpensesReportAggregate()
    {
        var expensesContext = new ExpensesContext();
        Expenses = expensesContext.Expenses.ToList();
        Id = 1;
    }

    public ExpensesReportAggregate(List<Expenses> expenses, int id)
    {
        this.Expenses = expenses;
        Id = id;
    }

    public List<Expense> RetrieveExpenseList()
    {
        return Expenses
            .Select(x => new Expense(x.ExpenseType, x.Amount))
            .ToList();
    }
}