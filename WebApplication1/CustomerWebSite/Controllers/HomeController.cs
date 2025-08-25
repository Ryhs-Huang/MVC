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
		[ValidateAntiForgeryToken]//�۰ʴ��J��������
		public IActionResult Contact([Bind("Name,Email,Phone")]ContactViewModel cvm)//Bind���d�L�ױi�K(���C�i�������A�������~���N�|�L�k��)
		{
			if (ModelState.IsValid) //Server���ҳq�L
			{
				//�g�J��Ʈw
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
