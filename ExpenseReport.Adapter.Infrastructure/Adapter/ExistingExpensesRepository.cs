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
    
    // FIXME: Do I need expenseList?
    public Domain.ExpenseReport? CreateAggregate(DateTimeOffset? expenseDate,
        List<CreateExpenseRequest> createExpenseRequests) 
    {
        var expenseReport = new ExpenseReport()
        {
            Expenses = createExpenseRequests
                .Select(x => 
                    new ExpenseDbo() { ExpenseType = x.type, Amount = x.amount })
                .ToList(),
            ExpenseReportDate = expenseDate ?? DateTimeOffset.Now
        };
        var entityEntry = expensesDbContext.ExpenseReport.Add(
            expenseReport);
        expensesDbContext.SaveChanges();
        expensesDbContext.ChangeTracker.Clear();
        return new Domain.ExpenseReport(
            createExpenseRequests
                .Select(x => 
                    new Domain.Expense(x.type, x.amount))
                .ToList(), 
            entityEntry.Entity?.ExpenseReportDate ?? DateTimeOffset.Now, 
            entityEntry.Entity?.Id ?? 0);
    }

    public Domain.ExpenseReport? UpdateAggregate(List<CreateExpenseRequest> createExpenseRequests)
    {
        var report = expensesDbContext
            .ExpenseReport
            .Include("Expenses")
            .SingleOrDefault(x => createExpenseRequests.Select(y => y.expenseReportId).Contains(x.Id));
        if (report?.Expenses != null && report.Expenses.Any())
        {
            report.Expenses.AddRange(createExpenseRequests.Select(x => 
                    new ExpenseDbo() { ExpenseType = x.type, Amount = x.amount })
                .ToList());
        }
        else
        {
            report.Expenses = createExpenseRequests.Select(x => 
                    new ExpenseDbo() { ExpenseType = x.type, Amount = x.amount })
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