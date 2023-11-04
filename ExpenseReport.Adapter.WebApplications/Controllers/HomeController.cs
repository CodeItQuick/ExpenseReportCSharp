using System.Diagnostics;
using WebApplication1.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ExpensesService _expenseService;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _expenseService = new ExpensesService(new RealDateProvider());
    }

    public ActionResult<ExpenseView> Index()
    {
        var viewExpenses = _expenseService.viewExpenses();
        return View(viewExpenses);
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
}