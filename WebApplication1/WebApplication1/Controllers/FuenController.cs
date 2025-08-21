using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
	public class FuenController : Controller
	{
		public IActionResult Index() //Action函式
		{
			return View();           //Index.cshtml
		}
	}
}
