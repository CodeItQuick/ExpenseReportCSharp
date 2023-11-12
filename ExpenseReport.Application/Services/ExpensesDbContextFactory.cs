using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services;

public class ExpensesDbContextFactory : IDesignTimeDbContextFactory<ExpensesDbContext>
{
    public ExpensesDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ExpensesDbContext>();
        optionsBuilder.UseSqlite("Data Source=blog.db");

        return new ExpensesDbContext(optionsBuilder.Options);
    }
}