
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using marian_onsite.Models;

namespace marian_onsite.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    public IActionResult Profile()
    {
        return View();
    }

    public IActionResult EditProfile()
    {
        return View();
    }

    public IActionResult StudyGroups ()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}