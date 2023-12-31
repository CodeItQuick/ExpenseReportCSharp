using System.ComponentModel.DataAnnotations;
using Domain;
using ExpenseReport.ApplicationServices;

// Not actually a service
namespace ExpenseReport.Adapter.WebBlazorServerApp.Data
{
    public class ExpenseReportAdapterService
    {
        private readonly ILogger<ExpenseReportAdapterService> _logger;
        private IExpenseService _expenseService;

        public ExpenseReportAdapterService(
            ILogger<ExpenseReportAdapterService> logger, 
            IExpenseService expensesService)
        {
            _logger = logger;
            _expenseService = expensesService; // FIXME: broken
        }
        public Task<ExpenseView> OnGet(int id = 1)
        {
            Domain.ExpenseReport? expenseReport = _expenseService.RetrieveExpenseReport(id, "abcd-1234");
            var expenseReportList = _expenseService.ListAllExpenseReports("abcd-1234");
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
            // FIXME: put this method somewhere - also this method is wrong
            List<string> displayExpenses = new List<string>();
            foreach (Expense expense in expenseReport.CalculateIndividualExpenses()) {
                String label = expense.ExpenseTypes() + "\t" + expense.Amount() + "\t" +
                               (expense.IsOverExpensedMeal() ? "X" : " ");
                displayExpenses.Add(label);
            }
            // FIXME: put this method somewhere - also this method is wrong
            var expenseView = new ExpenseView() 
            {
                MealExpenses = expenseReport.CalculateMealExpenses(),
                ExpenseDate = expenseReport.RetrieveDate(),
                TotalExpenses = expenseReport.CalculateTotalExpenses(),
                IndividualExpenses = displayExpenses,
                Id = expenseReport.Id,
                ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
            };
            return Task.FromResult(expenseView);
        }
        
        public Task<ExpenseView> ExpenseView(
            int expenseCost, string expenseType, int expenseReportId)
        {
            var tryParse = ExpenseType.TryParse(expenseType, out ExpenseType expense);
            var expenseAdded = _expenseService.AddExpenseToExpenseReport(
                expenseReportId, new List<CreateExpenseRequest>()
                {
                    new()
                    {
                        type = expense,
                        amount = expenseCost,
                        expenseReportId = expenseReportId,
                    }
                });
            var expenseReportList = _expenseService.ListAllExpenseReports("abcd-1234");

            // FIXME: put this method somewhere - also this method is wrong
            List<string> displayExpenses = new List<string>();
            foreach (Expense expensed in expenseAdded.CalculateIndividualExpenses()) {
                String label = expensed.ExpenseTypes() + "\t" + expensed.Amount() + "\t" +
                               (expensed.IsOverExpensedMeal() ? "X" : " ");
                displayExpenses.Add(label);
            }
            // FIXME: put this method somewhere - also this method is wrong
            return Task.FromResult(new ExpenseView()
            {
                MealExpenses = expenseAdded.CalculateMealExpenses(),
                ExpenseDate = expenseAdded.RetrieveDate(),
                IndividualExpenses = displayExpenses,
                TotalExpenses = expenseAdded.CalculateTotalExpenses(),
                Id = expenseAdded.Id,
                ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
            });
        }
        // Needs an endpoint
        public Task<ExpenseView> CreateExpenseReport([Required] DateTimeOffset expenseReportDate)
        {
            var expenseAdded = _expenseService.CreateExpenseReport(expenseReportDate, "abcd-1234");
            var expenseReportList = _expenseService.ListAllExpenseReports("abcd-1234");

            // FIXME: put this method somewhere - also this method is wrong
            List<string> displayExpenses = new List<string>();
            foreach (Expense expensed in expenseAdded.CalculateIndividualExpenses()) {
                String label = expensed.ExpenseTypes() + "\t" + expensed.Amount() + "\t" +
                               (expensed.IsOverExpensedMeal() ? "X" : " ");
                displayExpenses.Add(label);
            }
            // FIXME: put this method somewhere - also this method is wrong
            return Task.FromResult(new ExpenseView()
            {
                MealExpenses = expenseAdded.CalculateMealExpenses(),
                ExpenseDate = expenseAdded.RetrieveDate(),
                IndividualExpenses = displayExpenses,
                TotalExpenses = expenseAdded.CalculateTotalExpenses(),
                Id = expenseAdded.Id,
                ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
            });
        }
    }
}