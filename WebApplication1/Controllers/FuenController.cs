using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
	public class FuenController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
