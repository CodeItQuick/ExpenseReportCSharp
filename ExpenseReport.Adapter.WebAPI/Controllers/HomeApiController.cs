using Domain;
using ExpenseReport.ApplicationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseReport.Adapter.WebAPI.Controllers;

[ApiController]
[Route("Home")]
[Authorize("AllRegisteredUsers")]
public class HomeApiController : Controller
{
    private readonly ILogger<HomeApiController> _logger;
    private IExpenseService _expenseService;

    public HomeApiController(ILogger<HomeApiController> logger, IExpenseService expensesService)
    {
        _logger = logger;
        _expenseService = expensesService; // FIXME: broken
    }

    [HttpGet]
    public IActionResult Index(int id = 1)
    {
        Domain.ExpenseReport? expenseReport = _expenseService.RetrieveExpenseReport(id);
        var expenseReportList = _expenseService.ListAllExpenseReports();
        if (expenseReport == null)
        {
            return Ok(new ExpenseApiView()
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
        foreach (Expense expensed in expenseReport.CalculateIndividualExpenses()) {
            String label = expensed.ExpenseTypes() + "\t" + expensed.Amount() + "\t" +
                           (expensed.IsOverExpensedMeal() ? "X" : " ");
            displayExpenses.Add(label);
        }
        // FIXME: put this method somewhere - also this method is wrong
        var expenseView = new ExpenseApiView() 
        {
            MealExpenses = expenseReport.CalculateMealExpenses(),
            ExpenseDate = expenseReport.RetrieveDate(),
            TotalExpenses = expenseReport.CalculateTotalExpenses(),
            IndividualExpenses = displayExpenses,
            Id = expenseReport.Id,
            ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
        };
        return Ok(expenseView);
    }

    [HttpPost("ExpenseView")]
    public IActionResult ExpenseView(
        int expenseCost, string expenseType, int reportId = 0)
    {
        var tryParse = ExpenseType.TryParse(expenseType, out ExpenseType expense);
        var expenseAdded = _expenseService.AddExpenseToExpenseReport(reportId, 
            new List<CreateExpenseRequest>()
            {
                new()
                {
                    type = expense,
                    amount = expenseCost,
                    expenseReportId = reportId,
                }
            });
        var expenseReportList = _expenseService.ListAllExpenseReports();
        // FIXME: put this method somewhere - also this method is wrong
        List<string> displayExpenses = new List<string>();
        foreach (Expense expensed in expenseAdded.CalculateIndividualExpenses()) {
            String label = expensed.ExpenseTypes() + "\t" + expensed.Amount() + "\t" +
                           (expensed.IsOverExpensedMeal() ? "X" : " ");
            displayExpenses.Add(label);
        }
        // FIXME: put this method somewhere - also this method is wrong

        return Ok(new ExpenseApiView()
        {
            MealExpenses = expenseAdded.CalculateMealExpenses(),
            ExpenseDate = expenseAdded.RetrieveDate(),
            IndividualExpenses = displayExpenses,
            TotalExpenses = expenseAdded.CalculateTotalExpenses(),
            ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
        });
    }
    [HttpPost("ExpenseUpdateView")]
    public IActionResult ExpenseUpdateView(
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
        var expenseReportList = _expenseService.ListAllExpenseReports();

        // FIXME: put this method somewhere - also this method is wrong
        List<string> displayExpenses = new List<string>();
        foreach (Expense expensed in expenseAdded.CalculateIndividualExpenses()) {
            String label = expensed.ExpenseTypes() + "\t" + expensed.Amount() + "\t" +
                           (expensed.IsOverExpensedMeal() ? "X" : " ");
            displayExpenses.Add(label);
        }
        // FIXME: put this method somewhere - also this method is wrong
        return Ok(new ExpenseApiView()
        {
            MealExpenses = expenseAdded.CalculateMealExpenses(),
            ExpenseDate = expenseAdded.RetrieveDate(),
            IndividualExpenses = displayExpenses,
            TotalExpenses = expenseAdded.CalculateTotalExpenses(),
            Id = expenseAdded.Id,
            ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
        });
    }
    [HttpPost("CreateExpenseReport")]
    public IActionResult CreateExpenseReport(DateTimeOffset expenseReportDate)
    {
        var expenseAdded = _expenseService.CreateExpenseReport(expenseReportDate);
        var expenseReportList = _expenseService.ListAllExpenseReports();

        // FIXME: put this method somewhere - also this method is wrong
        List<string> displayExpenses = new List<string>();
        foreach (Expense expensed in expenseAdded.CalculateIndividualExpenses()) {
            String label = expensed.ExpenseTypes() + "\t" + expensed.Amount() + "\t" +
                           (expensed.IsOverExpensedMeal() ? "X" : " ");
            displayExpenses.Add(label);
        }
        // FIXME: put this method somewhere - also this method is wrong
        return Ok(new ExpenseApiView()
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