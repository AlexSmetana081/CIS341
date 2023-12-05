using Lab05.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lab05.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //Returns view of index page
        public IActionResult Index()
        {
            return View();
        }

        //Privacy page by default settings. Will remove later
        public IActionResult Privacy()
        {
            return View();
        }

    }
}