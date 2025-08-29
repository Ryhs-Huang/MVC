using Microsoft.AspNetCore.Mvc;

namespace Fuen43Team0.Areas.Accouting.Controllers
{
	[Area("Accouting")]//宣告是哪個Area的
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Depreciation()
		{
			return View();
		}

		public IActionResult WriteOff()
		{
			return View();
		}
	}
}
