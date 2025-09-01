using Microsoft.AspNetCore.Mvc;

namespace Fuen43Team0.Areas.Accounting.Controllers
{
	[Area("Accounting")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();      // Index.cshtml
		}

		public IActionResult Depreciation()
		{
			return View();      // Depreciation.cshtml
		}

		public IActionResult WriteOff()
		{
			return View();      // WriteOff.cshtml
		}
	}
}
