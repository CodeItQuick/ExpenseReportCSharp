using System.Security.Claims;
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
        var expenseView = new ExpenseApiView() 
        {
            MealExpenses = expenseReport?.CalculateMealExpenses() ?? 0,
            ExpenseDate = expenseReport?.RetrieveDate() ?? DateTimeOffset.Now,
            TotalExpenses = expenseReport?.CalculateTotalExpenses() ?? 0,
            IndividualExpenses = expenseReport != null ? ExpenseApiView.CreateTransformedExpenses(expenseReport) : new List<ExpenseDto>(),
            Id = expenseReport?.Id ?? 0,
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

        return Ok(new ExpenseApiView()
        {
            MealExpenses = expenseAdded.CalculateMealExpenses(),
            ExpenseDate = expenseAdded.RetrieveDate(),
            IndividualExpenses = ExpenseApiView.CreateTransformedExpenses(expenseAdded),
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

        return Ok(new ExpenseApiView()
        {
            MealExpenses = expenseAdded.CalculateMealExpenses(),
            ExpenseDate = expenseAdded.RetrieveDate(),
            IndividualExpenses = ExpenseApiView.CreateTransformedExpenses(expenseAdded),
            TotalExpenses = expenseAdded.CalculateTotalExpenses(),
            Id = expenseAdded.Id,
            ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
        });
    }
    [HttpPost("CreateExpenseReport")]
    public IActionResult CreateExpenseReport(DateTimeOffset expenseReportDate)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new ArgumentNullException("no user id");
        var expenseAdded = _expenseService.CreateExpenseReport(expenseReportDate, userId);
        var expenseReportList = _expenseService.ListAllExpenseReports();

        return Ok(new ExpenseApiView()
        {
            MealExpenses = expenseAdded.CalculateMealExpenses(),
            ExpenseDate = expenseAdded.RetrieveDate(),
            IndividualExpenses = ExpenseApiView.CreateTransformedExpenses(expenseAdded),
            TotalExpenses = expenseAdded.CalculateTotalExpenses(),
            Id = expenseAdded.Id,
            ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
        });
    }
}