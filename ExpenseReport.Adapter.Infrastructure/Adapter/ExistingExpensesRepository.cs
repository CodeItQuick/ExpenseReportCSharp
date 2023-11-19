using Domain;
using ExpenseReport.ApplicationServices;
using Microsoft.EntityFrameworkCore;

namespace Application.Adapter;

public class ExistingExpensesRepository : IExistingExpensesRepository
{
    protected ExpensesDbContext expensesDbContext;

    public ExistingExpensesRepository(IDateProvider dateProvider)
    {
        var dbContextOptions = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseSqlite("Data Source=.\\..\\blog.db")
            .Options; 
        expensesDbContext = new ExpensesDbContext(dbContextOptions);
        expensesDbContext.Database.EnsureCreated();
    }

    public ExistingExpensesRepository(ExpensesDbContext expensesDbContext, IDateProvider dateProvider)
    {
        this.expensesDbContext = expensesDbContext;
    }

    public Domain.ExpenseReport? RetrieveById(int id)
    {
        var expenseReport = expensesDbContext.ExpenseReport
            .Include("Expenses")
            .FirstOrDefault(x => x.Id == id);
        if (expenseReport == null)
        {
            return null;
        }
        return new Domain.ExpenseReport(
            expenseReport.Expenses?.Select(x => new Domain.Expense(x.ExpenseType, x.Amount)).ToList(), 
            expenseReport.ExpenseReportDate, 
            expenseReport?.Id ?? 0);
    }

    public Domain.ExpenseReport? CreateAggregate(List<Expense> expenseList, DateTimeOffset? expenseDate)
    {
        var expenseReport = new ExpenseReport()
        {
            Expenses = expenseList
                .Select(x => 
                    new ExpenseDbo() { ExpenseType = x.ExpenseTypes(), Amount = x.Amount() })
                .ToList(),
            ExpenseReportDate = expenseDate ?? DateTimeOffset.Now
        };
        var entityEntry = expensesDbContext.ExpenseReport.Add(
            expenseReport);
        expensesDbContext.SaveChanges();
        expensesDbContext.ChangeTracker.Clear();
        return new Domain.ExpenseReport(
            expenseList
                .Select(x => 
                    new Domain.Expense(x.ExpenseTypes(), x.Amount()))
                .ToList(), 
            entityEntry.Entity?.ExpenseReportDate ?? DateTimeOffset.Now, 
            entityEntry.Entity?.Id ?? 0);
    }

    public Domain.ExpenseReport? UpdateAggregate(List<Expense> expenses, int expenseReportId,
        List<CreateExpenseRequest> createExpenseRequests)
    {
        var report = expensesDbContext
            .ExpenseReport
            .Include("Expenses")
            .FirstOrDefault(x => x.Id == expenseReportId);
        if (report?.Expenses != null && report.Expenses.Any())
        {
            report.Expenses.AddRange(expenses.Select(x => 
                    new ExpenseDbo() { ExpenseType = x.ExpenseTypes(), Amount = x.Amount() })
                .ToList());
        }
        else
        {
            report.Expenses = expenses.Select(x => 
                    new ExpenseDbo() { ExpenseType = x.ExpenseTypes(), Amount = x.Amount() })
                .ToList();
        }
        expensesDbContext.ExpenseReport.Update(report);
        expensesDbContext.SaveChanges();
        return new Domain.ExpenseReport(
            report?.Expenses.Select(x => new Domain.Expense(x.ExpenseType, x.Amount)).ToList(), 
            report?.ExpenseReportDate ?? DateTimeOffset.Now, 
            report.Id);
    }

    public List<Domain.ExpenseReport> ListAllExpenseReports()
    {
        return expensesDbContext.ExpenseReport
            .Include("Expenses")
            .ToList()
            .Select(x => new Domain.ExpenseReport(
                x?.Expenses?.Select(x => new Domain.Expense(x.ExpenseType, x.Amount)).ToList() ?? new List<Domain.Expense>(), 
                x?.ExpenseReportDate ?? DateTimeOffset.Now, 
                x.Id
            )).ToList();
    }
}