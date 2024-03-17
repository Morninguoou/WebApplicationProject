
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using marian_onsite.Models;

namespace marian_onsite.Controllers;

public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(ILogger<ProfileController> logger)
    {
        _logger = logger;
    }
    

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult EditProfile()
    {
        return View();
    }

    public IActionResult Following ()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}