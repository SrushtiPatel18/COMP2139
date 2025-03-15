using System.Diagnostics;
using COMP2139_Lab02.Areas.ProjectManagement.Controllers;
using Microsoft.AspNetCore.Mvc;
using COMP2139_Lab02.Models;
using COMP2139_Labs.Areas.ProjectManagement.Controllers;

namespace COMP2139_Lab02.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    [HttpGet]
    public IActionResult GeneralSearch(string searchType, string searchString)
    {
        searchType = searchType?.Trim().ToLower();
        if (string.IsNullOrWhiteSpace(searchType) || string.IsNullOrWhiteSpace(searchString))
        {
            return RedirectToAction("Index", "Home");
        }

        if (searchType == "project")
        {
            return RedirectToAction(nameof(ProjectController.Search),
                "Project", new { area = "ProjectManagement"  ,searchString = searchString });
        }
        else if (searchType == "tasks")
        {
            return RedirectToAction(nameof(ProjectTaskController.Search),
                "ProjectTask", new { searchString = searchString });
        }
        return RedirectToAction("Index", "Home");
    }
}
