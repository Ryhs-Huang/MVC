using CustomerWebSite.Models;
using CustomerWebSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;

namespace CustomerWebSite.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		NorthwindContext _context;  // 拿到注入的物件，保留一份
		private IMemoryCache _cache;

		// Controller 要注入的話，就在建構函式的參數加入
		// 然後保留一份來用，就可以了
		public HomeController(ILogger<HomeController> logger, NorthwindContext context, IMemoryCache cache)
		{
			_logger = logger;
			_context = context;
			_cache = cache;
		}

		public IActionResult Index()
		{
			ViewBag.CustomerCounty = new SelectList(_context.Customers.Select(c => c.Country).Distinct());
			//ViewData["CustomerCounty"] = new SelectList(_context.Customers.Select(c => c.Country).Distinct());

			ViewBag.Script = $"alert('客戶人數: {_context.Customers.Count()} 人');";

			// 存資料到 Session
			HttpContext.Session.SetString("SessionKey", "SessionValue");
			// SetString(key, value) → 將字串存入 Session

			// 建立cache快取選項
			MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions();
			cacheOptions.SetSlidingExpiration(TimeSpan.FromDays(1));
			cacheOptions.SetPriority(CacheItemPriority.Normal);
			// 存入cache快取
			_cache.Set("CacheKey", "CacheValue", cacheOptions);

			// 建立cookie快取選項
			CookieOptions cookieOption = new CookieOptions();
			cookieOption.Expires = DateTime.Now.AddYears(30);
			cookieOption.HttpOnly = true;
			cookieOption.Secure = true;
			Response.Cookies.Append("CookieKey", "CookieValue", cookieOption);

			return View();  // Index.cshtml
		}

		public IActionResult Privacy()
		{
			// 取資料 
			string? SessionValue = HttpContext.Session.GetString("SessionKey");
			// GetString(key) → 從 Session 取出字串
			if (SessionValue != null) { }

			// 讀取cache快取資料
			string? CacheValue = _cache.Get<string>("CacheKey");
			if (CacheValue != null) { }

			// 讀取cookie快取資料
			string? CookieValue = Request.Cookies["CookieKey"];
			if (CookieValue != null) { }

			return View();  // Privacy.cshtml
		}

		public IActionResult Customers()
		{
			return View(_context.Customers);        // Customers.cshtml	

			//NorthwindContext context = new NorthwindContext();
			//return View(context.Customers);
		}

		// GET: /Home/Contact
		[HttpGet]
		public IActionResult Contact()
		{
			return View();  // 函式長這樣就是生畫面用的
		}

		// POST: /Home/Contact
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Contact([Bind("Name,Email")] ContactViewModel cvm) // 明列所有可以接收的欄位	// 白名單比黑名單安全，先白再黑																				
		{
			{
				// 黑名單寫法
				if (ModelState.IsValid) // Server端驗證通過
				{
					// 寫入資料庫

					//ViewBag.Message = "成功!";
					//return View();  // 同一個 Request，ViewBag 可以使用

					TempData["Message"] = "成功!";
					return RedirectToAction("Index", "Home");  // 告訴瀏覽器：「請去發送一個新的 GET 請求到 /Home/Index」。
															   // 觸發新 Request，必須用 TempData

					// 通過就轉到首頁
				}
				return View(cvm);  // 驗證失敗停留在這個畫面

				//return View(coll);
			}
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

	}
}