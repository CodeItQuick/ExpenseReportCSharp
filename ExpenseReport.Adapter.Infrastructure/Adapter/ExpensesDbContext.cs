using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Application.Adapter;

public sealed class ExpensesDbContext : IdentityDbContext
{
    public DbSet<ExpenseDbo> Expenses { get; set; }
    private readonly string _connectionString;
    public DbSet<ExpenseReport?> ExpenseReport { get; set; }
    private string DbPath { get; set; }

    public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=.\\..\\blog.db");
        }
    }
}

public class ExpenseContextFactory : IDesignTimeDbContextFactory<ExpensesDbContext>
{
    public ExpensesDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ExpensesDbContext>();
        optionsBuilder.UseSqlite("Data Source=.\\..\\blog.db");

        return new ExpensesDbContext(optionsBuilder.Options);
    }
}
