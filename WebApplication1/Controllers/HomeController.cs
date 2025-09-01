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

		// GET Home/Index
		[HttpGet]   // 預設的 HTTP 動作方法是 GET，是預設可以不寫，方便看而已
		public IActionResult Index()
		{
			return View();  // 預設回傳 Views/Home/Index.cshtml
		}

		// GET Home/Index
		// => uri 重複了，會錯誤
		//[HttpGet]
		//public IActionResult Index(int n)
		//{
		//	return View();  // 預設回傳 Views/Home/Index.cshtml
		//}

		// Post Home/Index
		// => uri 包含動詞，所以這個跟上面的 Index 方法不會衝突
		[HttpPost]
		public IActionResult Index(int n)
		{
			return View();  // 預設回傳 Views/Home/Index.cshtml
		}

		public IActionResult Privacy()
		{
			// 模擬一個例外（除以 0），實際執行時會進入錯誤處理流程
			//int x = 0;
			//int y = 10;
			//int z = y / x;

			return View();  // 預設回傳 Views/Home/Privacy.cshtml
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()   // 預設回傳 Views/Shared/Error.cshtml，並傳入 ErrorViewModel
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
