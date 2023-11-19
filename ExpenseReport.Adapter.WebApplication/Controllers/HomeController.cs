using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Application.Services;
using Domain;
using ExpenseReport.ApplicationServices;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

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
        // List of Expense Reports
        // Sort them
        // Filter them
        // Sum all the expense reports by manager
        
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

    // Create
    public ActionResult ExpenseView(
        int expenseCost, string expenseType, int reportId = 0)
    {
        var tryParse = ExpenseType.TryParse(expenseType, out ExpenseType expense);
        var expenseAdded = _expenseService.AddExpenseToExpenseReport(
            reportId, new Expense(expense, expenseCost));
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
        var expenseAdded = _expenseService.AddExpenseToExpenseReport(
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