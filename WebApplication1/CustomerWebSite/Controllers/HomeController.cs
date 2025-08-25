using CustomerWebSite.Models;
using CustomerWebSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CustomerWebSite.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		NorthwindContext _context;

		public HomeController(ILogger<HomeController> logger, NorthwindContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult Customers() { 
		return View(_context.Customers);
		}

		//Get /Home/Contact
		[HttpGet]
		public IActionResult Contact()
		{
			return View();   //Contact
		}

		//Post /Home/Contact
		[HttpPost]
		[ValidateAntiForgeryToken]//自動插入防偽標籤
		public IActionResult Contact([Bind("Name,Email,Phone")]ContactViewModel cvm)//Bind防範過度張貼(明列可接收欄位，除此之外的就會無法傳)
		{
			if (ModelState.IsValid) //Server驗證通過
			{
				//寫入資料庫
				return RedirectToAction("Index", "Home");
			}

			return View(cvm);   //Contact
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
