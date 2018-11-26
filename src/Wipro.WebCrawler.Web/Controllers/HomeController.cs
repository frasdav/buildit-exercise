using Microsoft.AspNetCore.Mvc;

namespace Wipro.WebCrawler.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string url)
        {
            return View();
        }
    }
}