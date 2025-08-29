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
            return View(_context.Categories.Select(c=>new Category
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Picture=null
            }));
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.Select(c=>new Category
            {
                CategoryId=c.CategoryId,
                CategoryName=c.CategoryName,
                Description=c.Description,
                Picture=null

            }).FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //Get: Categories/GetPicture/1
        [HttpGet]
        public async Task<FileResult> GetPicture(int id)
        {
            Category? c =await _context.Categories.FindAsync(id);
            byte[]? ImageData = c?.Picture;
            return File(ImageData, "image/jpeg");
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
				if (Request.Form.Files["Picture"] != null) //如果有上傳圖片，做覆蓋
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
		[RequestFormLimits(MultipartBodyLengthLimit = 2048000)]
		[RequestSizeLimit(2048000)]
		public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Description,Picture")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Category? c=await _context.Categories.FindAsync(category.CategoryId);
                if (Request.Form.Files["Picture"] != null) //如果有上傳圖片，做覆蓋
                {
					ReadUploadImage(category);
				}
                else
                {
                    category.Picture = c.Picture;//如果使用者未上傳圖片，則維持原圖(無上傳不可覆蓋)
                }
                _context.Entry(c).State = EntityState.Detached;//卸離c，只追蹤category(因開頭有做FindAsync查找圖片)

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

            var category = await _context.Categories.Select(c=>new Category
            {
				CategoryId = c.CategoryId,
				CategoryName = c.CategoryName,
                Description= c.Description,
                Picture=null
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
