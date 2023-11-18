using System.ComponentModel.DataAnnotations;
using Domain;
using ExpenseReport.ApplicationServices;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseReport.Adapter.WebAPI.Controllers;

[ApiController]
[Route("Home")]
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
        var expenseView = new ExpenseApiView() 
        {
            MealExpenses = expenseReport.CalculateMealExpenses(),
            ExpenseDate = expenseReport.RetrieveDate(),
            TotalExpenses = expenseReport.CalculateTotalExpenses(),
            IndividualExpenses = expenseReport.CalculateIndividualExpenses(),
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
        var expenseAdded = _expenseService.CreateExpense(reportId, new Expense(expense, expenseCost));
        var expenseReportList = _expenseService.ListAllExpenseReports();

        return Ok(new ExpenseApiView()
        {
            MealExpenses = expenseAdded.CalculateMealExpenses(),
            ExpenseDate = expenseAdded.RetrieveDate(),
            IndividualExpenses = expenseAdded.CalculateIndividualExpenses(),
            TotalExpenses = expenseAdded.CalculateTotalExpenses(),
            ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
        });
    }
    [HttpPost("ExpenseUpdateView")]
    public IActionResult ExpenseUpdateView(
        int expenseCost, string expenseType, int expenseReportId)
    {
        var tryParse = ExpenseType.TryParse(expenseType, out ExpenseType expense);
        var expenseAdded = _expenseService.CreateExpense(
            expenseReportId, new Expense(expense, expenseCost));
        var expenseReportList = _expenseService.ListAllExpenseReports();

        return Ok(new ExpenseApiView()
        {
            MealExpenses = expenseAdded.CalculateMealExpenses(),
            ExpenseDate = expenseAdded.RetrieveDate(),
            IndividualExpenses = expenseAdded.CalculateIndividualExpenses(),
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

        return Ok(new ExpenseApiView()
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