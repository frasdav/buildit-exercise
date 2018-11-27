using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Wipro.WebCrawler.Common.Interfaces;

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

            var results = await _webCrawler.CrawlAsync(new Uri(url.Contains("://") ? url : $"http://{url}"));

            ViewBag.Data = JsonConvert.SerializeObject(results, Formatting.Indented);

            return View();
        }
    }
}