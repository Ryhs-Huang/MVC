using CustomerWebSite.Models;
using CustomerWebSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace CustomerWebSite.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		NorthwindContext _context;
		IMemoryCache _cache;

		public HomeController(ILogger<HomeController> logger, NorthwindContext context, IMemoryCache cache)
		{
			_logger = logger;
			_context = context;
			_cache = cache;
		}

		public IActionResult Index()
		{
			ViewBag.CustomerCountry = new SelectList(_context.Customers.Select(c => c.Country).Distinct());
			//ViewData["CustomerCountry"] = new SelectList(_context.Customers.Select(c => c.Country).Distinct());
			ViewBag.Script = $"alert('客戶人數:{_context.Customers.Count()}')";
			HttpContext.Session.SetString("Sessionkey", "SessionValue");

			MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions();
			cacheOptions.SetSlidingExpiration(TimeSpan.FromDays(1));
			cacheOptions.SetPriority(CacheItemPriority.Normal);
			_cache.Set<string>("CacheKey", "CacheValue");

			CookieOptions cookieOptions = new CookieOptions();
			cookieOptions.Expires = DateTime.Now.AddYears(30);
			cookieOptions.HttpOnly=true;
			cookieOptions.Secure = true;
			Response.Cookies.Append("CookieKey", "CookieValue", cookieOptions);
			return View();
		}

		public IActionResult Privacy()
		{
			string? SessionValue = HttpContext.Session.GetString("SessionKey");
			if (SessionValue != null) 
			{
			}
			string? CacheValue = _cache.Get<string>("CacheKey");
			if ((CacheValue !=null))
			{	
			}
			string? CookieValue = Request.Cookies["CookieKey"];
			if ((CookieValue != null))
			{
			}
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
				TempData["Message"] = "成功";
				return RedirectToAction("Index", "Home");//下一個Request
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
