﻿using System.Diagnostics;
using Application.Adapter;
using WebApplication1.Models;
using Application.Services;
using Domain;
using ExpenseReport.ApplicationServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ExpensesService _expenseService;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _expenseService = new ExpensesService(
            new RealDateProvider(), 
            new ExistingExpensesRepository(new RealDateProvider())); // FIXME: broken
    }

    public IActionResult Index()
    {
        Domain.ExpenseReport? expenseReport = _expenseService.RetrieveExpenseReport();
        if (expenseReport == null)
        {
            return View(new ExpenseView()
            {
                MealExpenses = 0,
                ExpenseDate = DateTimeOffset.Now,
                TotalExpenses = 0,
                IndividualExpenses = new List<string>()
            });
        }
        var expenseView = new ExpenseView() 
        {
            MealExpenses = expenseReport.CalculateMealExpenses(),
            ExpenseDate = expenseReport.RetrieveDate(),
            TotalExpenses = expenseReport.CalculateTotalExpenses(),
            IndividualExpenses = expenseReport.CalculateIndividualExpenses(),
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
        int expenseCost, string expenseType, DateTimeOffset expenseDate)
    {
        var tryParse = ExpenseType.TryParse(expenseType, out ExpenseType expense);
        var expenseAdded = _expenseService.CreateExpense(expenseCost, expense, expenseDate);

        return View("Index", new ExpenseView()
        {
            MealExpenses = expenseAdded.CalculateMealExpenses(),
            ExpenseDate = expenseAdded.RetrieveDate(),
            IndividualExpenses = expenseAdded.CalculateIndividualExpenses(),
            TotalExpenses = expenseAdded.CalculateTotalExpenses()
        });
    }

    public ViewResult CreateExpenseReport(DateTimeOffset expenseReportDate)
    {
        var expenseAdded = _expenseService.CreateNewExpenseReport(expenseReportDate);

        return View("Index", new ExpenseView()
        {
            MealExpenses = expenseAdded.CalculateMealExpenses(),
            ExpenseDate = expenseAdded.RetrieveDate(),
            IndividualExpenses = expenseAdded.CalculateIndividualExpenses(),
            TotalExpenses = expenseAdded.CalculateTotalExpenses()
        });
    }
}