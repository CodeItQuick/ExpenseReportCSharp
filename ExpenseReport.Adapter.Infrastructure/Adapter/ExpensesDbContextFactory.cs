using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Application.Adapter;

public class ExpensesDbContextFactory : IDesignTimeDbContextFactory<ExpensesDbContext>
{
    public ExpensesDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ExpensesDbContext>();
        optionsBuilder.UseSqlite("Data Source=blog.db");

        return new ExpensesDbContext(optionsBuilder.Options);
    }
}