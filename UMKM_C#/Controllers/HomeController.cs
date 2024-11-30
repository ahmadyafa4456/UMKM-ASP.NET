using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UMKM_C_.Models;

namespace UMKM_C_.Controllers
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
            return View();
        }
    }
}
