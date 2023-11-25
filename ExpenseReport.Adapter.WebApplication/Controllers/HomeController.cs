using System.Collections;
using System.Diagnostics;
using System.Security.Claims;
using Domain;
using ExpenseReport.ApplicationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IExpenseService _expenseService;

    public HomeController(ILogger<HomeController> logger, IExpenseService expensesService)
    {
        _logger = logger;
        _expenseService = expensesService; // FIXME: broken
    }

    public IActionResult Index(int id = 1)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new ArgumentNullException(nameof(userId));
        }
        Domain.ExpenseReport? expenseReport = _expenseService.RetrieveExpenseReport(id, userId);
        var expenseReportList = _expenseService.ListAllExpenseReports();
        
        var expenseView = new ExpenseView() 
        {
            MealExpenses = expenseReport?.CalculateMealExpenses() ?? 0,
            ExpenseDate = expenseReport?.RetrieveDate() ?? DateTimeOffset.Now,
            TotalExpenses = expenseReport?.CalculateTotalExpenses() ?? 0,
            IndividualExpenses = expenseReport != null ? Controllers.ExpenseView.CreateTransformedExpenses(expenseReport) : new List<ExpenseDto>(),
            Id = expenseReport?.Id ?? 0,
            ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList(),
            EmployeeId = expenseReport?.EmployeeId ?? ""
        };
        return View(expenseView);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Create
    public ActionResult ExpenseView(
        int expenseCost, string expenseType, int reportId = 0)
    {
        var tryParse = ExpenseType.TryParse(expenseType, out ExpenseType expense);
        var expenseAdded = _expenseService.AddExpenseToExpenseReport(
            reportId, new List<CreateExpenseRequest>()
            {
                new()
                {
                    type = expense,
                    amount = expenseCost,
                    expenseReportId = reportId,
                }
            });
        var expenseReportList = _expenseService.ListAllExpenseReports();

        return View("Index", new ExpenseView()
        {
            MealExpenses = expenseAdded.CalculateMealExpenses(),
            ExpenseDate = expenseAdded.RetrieveDate(),
            IndividualExpenses = Controllers.ExpenseView.CreateTransformedExpenses(expenseAdded),
            TotalExpenses = expenseAdded.CalculateTotalExpenses(),
            ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
        });
    }
    public ActionResult ExpenseUpdateView(
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

        return View("Index", new ExpenseView()
        {
            MealExpenses = expenseAdded.CalculateMealExpenses(),
            ExpenseDate = expenseAdded.RetrieveDate(),
            IndividualExpenses = Controllers.ExpenseView.CreateTransformedExpenses(expenseAdded),
            TotalExpenses = expenseAdded.CalculateTotalExpenses(),
            Id = expenseAdded.Id,
            ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
        });
    }

    public ViewResult CreateExpenseReport(DateTimeOffset? expenseReportDate)
    {
        var expenseAdded = _expenseService.CreateExpenseReport(
            expenseReportDate ?? DateTimeOffset.Now, 
            User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var expenseReportList = _expenseService.ListAllExpenseReports();

        return View("Index", new ExpenseView()
        {
            MealExpenses = expenseAdded.CalculateMealExpenses(),
            ExpenseDate = expenseAdded.RetrieveDate(),
            IndividualExpenses = Controllers.ExpenseView.CreateTransformedExpenses(expenseAdded),
            TotalExpenses = expenseAdded.CalculateTotalExpenses(),
            Id = expenseAdded.Id,
            EmployeeId = expenseAdded.EmployeeId,
            ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
        });
    }
}