using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Wipro.WebCrawler.Interfaces;

namespace Wipro.WebCrawler.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebCrawler _webCrawler;

        public HomeController(IWebCrawler webCrawler)
        {
            _webCrawler = webCrawler;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string url)
        {
            if (string.IsNullOrEmpty(url)) return View();

            var results = await _webCrawler.CrawlAsync(new Uri(url));

            ViewBag.Data = JsonConvert.SerializeObject(results.Select(r => new { url = r.Key, type = r.Value }), Formatting.Indented);

            return View();
        }
    }
}