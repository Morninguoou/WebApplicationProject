using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using marian_onsite.Models;


namespace marian_onsite.Controllers
{
    public class StudyGroupsController : Controller
    {
        private readonly ILogger<StudyGroupsController> _logger;

        public StudyGroupsController(ILogger<StudyGroupsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateStudyGroup()
        {
            return View();
        }

        public IActionResult MyStudyGroups()
        {
            return View();
        }

        public IActionResult StudyGroupDetails(string id)
        {
            ViewData["id"] = id;
            return View();
        }

        public IActionResult EditStudyGroupDetails(string id)
        {
            ViewData["id"] = id;
            return View();
        }


        public IActionResult VerifyStudyGroup(string id)
        {
            ViewData["id"] = id;
            return View();
        }
    }
}