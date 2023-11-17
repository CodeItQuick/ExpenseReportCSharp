using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Domain;
using ExpenseReport.Adapter.WebBlazorServerApp.Controllers;
using ExpenseReport.Adapter.WebBlazorServerApp.Data;
using ExpenseReport.ApplicationServices;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseReport.Adapter.WebBlazorServerApp;

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
        Domain.ExpenseReport? expenseReport = _expenseService.RetrieveExpenseReport(id);
        var expenseReportList = _expenseService.ListAllExpenseReports();
        if (expenseReport == null)
        {
            return View(new ExpenseView()
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

    public ActionResult ExpenseView(
        int expenseCost, string expenseType, DateTimeOffset expenseDate, int reportId = 0)
    {
        var tryParse = ExpenseType.TryParse(expenseType, out ExpenseType expense);
        var expenseAdded = _expenseService.CreateExpense(new Expense(expense, expenseCost), expenseDate);
        var expenseReportList = _expenseService.ListAllExpenseReports();

        return View("Index", new ExpenseView()
        {
            MealExpenses = expenseAdded.CalculateMealExpenses(),
            ExpenseDate = expenseAdded.RetrieveDate(),
            IndividualExpenses = expenseAdded.CalculateIndividualExpenses(),
            TotalExpenses = expenseAdded.CalculateTotalExpenses(),
            ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
        });
    }
    public ActionResult ExpenseUpdateView(
        int expenseCost, string expenseType, int expenseReportId)
    {
        var tryParse = ExpenseType.TryParse(expenseType, out ExpenseType expense);
        var expenseAdded = _expenseService.CreateExpense(
            expenseReportId, new Expense(expense, expenseCost));
        var expenseReportList = _expenseService.ListAllExpenseReports();

        return View("Index", new ExpenseView()
        {
            MealExpenses = expenseAdded.CalculateMealExpenses(),
            ExpenseDate = expenseAdded.RetrieveDate(),
            IndividualExpenses = expenseAdded.CalculateIndividualExpenses(),
            TotalExpenses = expenseAdded.CalculateTotalExpenses(),
            Id = expenseAdded.Id,
            ExpenseReportIds = expenseReportList.Select(x => x.Id).ToList()
        });
    }

    // FIXME: Needs a test and endpoint
    public ViewResult CreateExpenseReport([Required] DateTimeOffset expenseReportDate)
    {
        var expenseAdded = _expenseService.CreateExpenseReport(expenseReportDate);
        var expenseReportList = _expenseService.ListAllExpenseReports();

        return View("Index", new ExpenseView()
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