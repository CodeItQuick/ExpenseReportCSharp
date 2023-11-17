using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Application.Adapter;

public sealed class ExpensesDbContext : DbContext
{
    public DbSet<Expense> Expenses { get; set; }
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
            optionsBuilder.UseSqlite("Data Source=blog.db");
        }
    }
}

public class ExpenseContextFactory : IDesignTimeDbContextFactory<ExpensesDbContext>
{
    public ExpensesDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ExpensesDbContext>();
        optionsBuilder.UseSqlite("Data Source=blog.db");

        return new ExpensesDbContext(optionsBuilder.Options);
    }
}
