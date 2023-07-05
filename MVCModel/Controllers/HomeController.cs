using DTO.Models;
using Microsoft.AspNetCore.Mvc;
using MVCModel.Extensions;
using MVCModel.Models;
using System.Diagnostics;

namespace MVCModel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var user = HttpContext.Session.GetObject<UserAuthModel>("user");
            if(user == null)
            {
                return RedirectToAction("LogIn", "Auth");
            }
            return View(user);
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
}