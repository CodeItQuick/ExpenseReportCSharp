using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ExpensesContext : DbContext
{
    public DbSet<Expenses> Expenses { get; set; }
    public DbSet<ExpenseReportAggregate?> ExpenseReportAggregates { get; set; }
    private string DbPath { get; } = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "blogging.db");

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}