using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CategoryProducts.Models;

namespace CategoryProducts.Controllers
{
	public class CategoriesController : Controller
	{
		private readonly NorthwindContext _context;

		public CategoriesController(NorthwindContext context)
		{
			_context = context;
		}

		// GET: Categories
		public async Task<IActionResult> Index()
		{
			return View(_context.Categories.Select(c => new Category()
			{
				CategoryId = c.CategoryId,
				CategoryName = c.CategoryName,
				Description = c.Description,
				Picture = null
			}));
		}

		// GET: Categories/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = await _context.Categories.Select(c => new Category
			{
				CategoryId = c.CategoryId,
				CategoryName = c.CategoryName,
				Description = c.Description,
				Picture = null
			}).FirstOrDefaultAsync(m => m.CategoryId == id);
			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		// GET: Categories/GetPicture/1
		[HttpGet]
		public async Task<FileResult> GetPicture(int id)
		{
			Category? c = await _context.Categories.FindAsync(id); // FindAsync 是依據主索引鍵來 find

			byte[]? ImageData = c?.Picture;         // 這樣比較安全，c 如果沒有值就不會去取 Picture、填 null，有值才去取 Picture
													// 如果有替代圖案就寫 byte[]? ImageData = c == null ? null : (c.Picture ?? c.Picture2);
			return File(ImageData, "image/jpeg");   // 建成 jpeg 格式的檔案傳回去
		}


		// GET: Categories/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Categories/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Description,Picture")] Category category)
		{
			if (ModelState.IsValid)
			{
				if (Request.Form.Files["Picture"] != null)
				{
					ReadUploadImage(category);
				}
				_context.Add(category);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(category);
		}

		private void ReadUploadImage(Category category)
		{
			using (BinaryReader reader = new BinaryReader(Request.Form.Files["Picture"].OpenReadStream()))
			{
				category.Picture = reader.ReadBytes((int)Request.Form.Files["Picture"].Length);
			}
		}

		// GET: Categories/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = await _context.Categories.Select(c => new Category
			{
				CategoryId = c.CategoryId,
				CategoryName = c.CategoryName,
				Description = c.Description,
				Picture = null
			}).FirstOrDefaultAsync(m => m.CategoryId == id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}

		// POST: Categories/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[RequestFormLimits(MultipartBodyLengthLimit = 2048000)] // ≈ 2 MB。控制 表單 multipart 內容。
		[RequestSizeLimit(2048000)]                             // ≈ 2 MB。控制 整個 HTTP 請求大小。
		public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Description,Picture")] Category category)
		{
			if (id != category.CategoryId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				Category? c = await _context.Categories.FindAsync(category.CategoryId); // 從資料庫讀出目前的資料(要修改前的資料)
				if (Request.Form.Files["Picture"] != null)
				{
					ReadUploadImage(category);
				}
				else
				{
					category.Picture = c.Picture;
				}
				_context.Entry(c).State = EntityState.Detached;
				// 將從資料庫讀出的 c 物件設為 Detached，解除 EF Core 的追蹤
				// 因為 category 是從表單綁定的新物件，如果不解除追蹤，_context 同時追蹤兩個相同 CategoryId 的物件
				// 會導致 DbUpdateConcurrencyException: "The instance of entity type 'Category' cannot be tracked..."
				// 解除追蹤後，我們可以安全地使用 _context.Update(category) 來更新資料

				try
				{
					_context.Update(category);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CategoryExists(category.CategoryId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(category);
		}

		// GET: Categories/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = await _context.Categories.Select(c => new Category
			{
				CategoryId = c.CategoryId,
				CategoryName = c.CategoryName,
				Description = c.Description,
				Picture = null
			}).FirstOrDefaultAsync(m => m.CategoryId == id);
			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		// POST: Categories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var category = await _context.Categories.FindAsync(id);
			if (category != null)
			{
				_context.Categories.Remove(category);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool CategoryExists(int id)
		{
			return _context.Categories.Any(e => e.CategoryId == id);
		}
	}
}
