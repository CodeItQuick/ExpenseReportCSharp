using Application.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Adapter;

public class ExpensesDbContext : DbContext
{
    public DbSet<Expenses> Expenses { get; set; }
    public DbSet<ExpenseReportAggregate?> ExpenseReportAggregates { get; set; }
    private string DbPath { get; set; }

    public ExpensesDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
    }
}