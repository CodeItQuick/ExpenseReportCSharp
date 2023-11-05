using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ExpensesContext : DbContext
{
    public DbSet<Expenses> Expenses { get; set; }
    public DbSet<ExpenseReportAggregate?> ExpenseReportAggregates { get; set; }
    private string DbPath { get; set; }

    public ExpensesContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
    }
}