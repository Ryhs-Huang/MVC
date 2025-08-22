using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerWebSite.Controllers
{
	public class Home1Controller : Controller
	{
		// GET: Home1Controller
		public ActionResult Index()
		{
			return View();
		}

		// GET: Home1Controller/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: Home1Controller/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Home1Controller/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: Home1Controller/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: Home1Controller/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: Home1Controller/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Home1Controller/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
