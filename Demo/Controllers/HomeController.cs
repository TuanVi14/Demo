using System.Diagnostics;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
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
            var products = new List<Product>()
            {
        new Product { Id = 1, Name = "Áo thun", Price = 150000, Image = "/images/aothun.jpg" },
        new Product { Id = 2, Name = "Qu?n jean", Price = 300000, Image = "/images/jean.jpg" },
        new Product { Id = 3, Name = "Áo hoodie", Price = 400000, Image = "/images/hoodie.jpg" }
            };

            return View(products);
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
