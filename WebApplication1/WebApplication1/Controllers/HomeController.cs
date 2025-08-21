using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //Get Home/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();  //Index.cshtml
        }

        //uri不可重複
        //建議寫上預設值動詞
        //確認動詞的同時可避免重複uri

        /*
        //Post Home/Index
        [HttpPost]
        public IActionResult Index(int n)
        {
            return View();  //Index.cshtml
        }
        */

        //Get Home/Privacy
        [HttpGet]
        public IActionResult Privacy()
        {
            //int x =0;
            //int y= 10;
            //int z = y / x;
            return View();  //Privacy.cshtml
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
