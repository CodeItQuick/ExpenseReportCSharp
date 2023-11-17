using Domain;
using ExpenseReport.ApplicationServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseReport.Adapter.WebBlazorServerApp.Data
{
    public class ExpenseReportService
    {
        private readonly ILogger<ExpenseReportService> _logger;
        private IExpenseService _expenseService;

        public ExpenseReportService(ILogger<ExpenseReportService> logger, IExpenseService expensesService)
        {
            _logger = logger;
            _expenseService = expensesService; // FIXME: broken
        }
        public Task<ExpenseView> OnGet(int id = 1)
        {
            Domain.ExpenseReport? expenseReport = _expenseService.RetrieveExpenseReport(id);
            var expenseReportList = _expenseService.ListAllExpenseReports();
            if (expenseReport == null)
            {
                return Task.FromResult(new ExpenseView()
                {
                    MealExpenses = 0,
                    ExpenseDate = DateTimeOffset.Now,
                    TotalExpenses = 0,
                    IndividualExpenses = new List<string>(),
                    ExpenseReportIds = new List<int>()
                });
            }
            var expenseView = new ExpenseView() 
            {
                MealExpenses = expenseReport.CalculateMealExpenses(),
                ExpenseDate = expenseReport.RetrieveDate(),
                TotalExpenses = expenseReport.CalculateTotalExpenses(),
                IndividualExpenses = expenseReport.CalculateIndividualExpenses(),
                Id = expenseReport.Id,
                ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
            };
            return Task.FromResult(expenseView);
        }
        
        public Task<ExpenseView> ExpenseView(
            int expenseCost, string expenseType, int expenseReportId)
        {
            var tryParse = ExpenseType.TryParse(expenseType, out ExpenseType expense);
            var expenseAdded = _expenseService.CreateExpense(
                expenseReportId, new Expense(expense, expenseCost));
            var expenseReportList = _expenseService.ListAllExpenseReports();

            return Task.FromResult(new ExpenseView()
            {
                MealExpenses = expenseAdded.CalculateMealExpenses(),
                ExpenseDate = expenseAdded.RetrieveDate(),
                IndividualExpenses = expenseAdded.CalculateIndividualExpenses(),
                TotalExpenses = expenseAdded.CalculateTotalExpenses(),
                Id = expenseAdded.Id,
                ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
            });
        }
    }
}