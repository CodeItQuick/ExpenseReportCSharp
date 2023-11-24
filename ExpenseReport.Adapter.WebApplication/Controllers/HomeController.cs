using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Domain;
using ExpenseReport.ApplicationServices;
using Microsoft.AspNetCore.Authorization;
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
        Domain.ExpenseReport? expenseReport = _expenseService.RetrieveExpenseReport(id);
        var expenseReportList = _expenseService.ListAllExpenseReports();
        if (expenseReport == null)
        {
            return View(new ExpenseView()
            {
                MealExpenses = 0,
                ExpenseDate = DateTimeOffset.Now,
                TotalExpenses = 0,
                IndividualExpenses = new List<ExpenseDto>(),
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
            IndividualExpenses = Controllers.ExpenseView.CreateTransformedExpenses(expenseReport),
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

        // FIXME: put this method somewhere - also this method is wrong
        List<string> displayExpenses = new List<string>();
        foreach (Expense expensed in expenseAdded.CalculateIndividualExpenses()) {
            String label = expensed.ExpenseTypes() + "\t" + expensed.Amount() + "\t" +
                           (expensed.IsOverExpensedMeal() ? "X" : " ");
            displayExpenses.Add(label);
        }
        // FIXME: put this method somewhere - also this method is wrong
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
        var expenseAdded = _expenseService.CreateExpenseReport(expenseReportDate ?? DateTimeOffset.Now);
        var expenseReportList = _expenseService.ListAllExpenseReports();

        // FIXME: put this method somewhere - also this method is wrong
        List<string> displayExpenses = new List<string>();
        foreach (Expense expensed in expenseAdded.CalculateIndividualExpenses()) {
            String label = expensed.ExpenseTypes() + "\t" + expensed.Amount() + "\t" +
                           (expensed.IsOverExpensedMeal() ? "X" : " ");
            displayExpenses.Add(label);
        }
        // FIXME: put this method somewhere - also this method is wrong
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
}