using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CustomerWebSite.Models;

namespace CustomerWebSite.Controllers
{
    [Route("/Customers/{action=Index}/{CustomersID?}")]
    public class CustomersController : Controller
    {
        private readonly NorthwindContext _context;

        public CustomersController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
			// async 非同步
			// 用 async 通常會用 Task<IActionResult> 作為回傳型別
			return View(_context.Customers);
			// 把 _context.Customers 丟給 View

			// 有做非同步作業，一定要做非同步等待
			// await 就是非同步等待

			//return View(await _context.Customers.ToListAsync());
			//return View(await _context.Customers.ToArrayAsync());
			// .ToListAsync() 是轉集合
			// 也可以轉陣列 .ToArrayAsync()
			// 如果可以不要轉集合/陣列，盡量不要轉
		}

		// GET: Customers/Details/5
		public async Task<IActionResult> Details(string CustomerID)
        {
            if (CustomerID == null)
            {
                return NotFound();  // 404
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == CustomerID);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();  // Create.cshtml    // 生畫面的
		}

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]  
        public async Task<IActionResult> Create([Bind("CustomerId,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
		{                           
			if (ModelState.IsValid)     
            {
                // Server 端驗證
				// _context 是資料內容類別的物件
				_context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // 如果驗證成功，回到 Index 頁
			}
            return View(customer);              // 如果驗證失敗，顯示 Create 畫面
		}

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(string CustomerID)
        {
            if (CustomerID == null)
            {
                return NotFound();  // 404
            }

            var customer = await _context.Customers.FindAsync(CustomerID);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string CustomerID, [Bind("CustomerId,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
        {
            if (CustomerID != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(string CustomerID)
        {
            if (CustomerID == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == CustomerID);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

		// POST: Customers/DeleteConfirmed/5 
        // [HttpPost] 
        // 如果是這樣寫，刪除表單 (Delete.cshtml 裡的 <form asp-action="Delete">) 會對不到，因為它還是指向 /Delete。 
        // 要改成<form asp-action = "DeleteConfirmed" > 才能對應。

		// POST: Customers/Delete/5
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string CustomerID)
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(string CustomerID)
        {
            return _context.Customers.Any(e => e.CustomerId == CustomerID);
        }
    }
}
