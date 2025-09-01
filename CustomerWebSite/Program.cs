using CustomerWebSite.Data;
using CustomerWebSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomerWebSite
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the DI container.
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
				?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			builder.Services.AddDbContext<NorthwindContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("Northwind"));
				// Configuration 會自動去 appsettings.json 找 ConnectionStrings -> NorthwindDatabase
			});

			// 註冊 Session 服務
			// AddSession()：註冊 Session 服務，讓應用程式可以使用 HttpContext.Session。
			builder.Services.AddSession(option =>
			{
				option.Cookie.Name = ".CustomerWebSite.Session";    // 設定用來存放 Session ID 的 Cookie 名稱，瀏覽器會存這個 Cookie
				option.IdleTimeout = TimeSpan.FromMinutes(5);		// 設定 Session 的逾時時間
				option.Cookie.IsEssential = true;					// 設定 Cookie 為必要，確保在未同意 Cookie 政策時仍然可用

				// 以下是安全性相關設定
				option.Cookie.HttpOnly = true;								// Cookie 只能透過 HTTP 存取，JavaScript 無法讀寫，防範 XSS
				option.Cookie.SecurePolicy = CookieSecurePolicy.Always;     // 強制 Cookie 只能在 HTTPS 連線下傳送，防止明文傳輸被截取

			});
			 
			// 註冊分散式快取 (Distributed Cache) 的記憶體實作，Session 需要這個服務來儲存 Session 資料
			builder.Services.AddDistributedMemoryCache();

			builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.AddControllersWithViews();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();      // → 自動跳轉到 HTTPS
			app.UseStaticFiles();           // → 提供靜態檔案服務
			app.UseRouting();               // → 路由中間件
			app.UseAuthorization();         // → 權限驗證

			// 啟用 Session 中間件
			app.UseSession();               // → 必須放在 UseRouting 之後、UseEndpoints 之前，確保每個 Request 都能使用 Session。

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			// app.MapControllerRoute(
			//	 name: "CustomerRoute",
			//	 pattern: "{controller=Home}/{action=Index}/{CustomerId?}");
			// 不在此定義，因為這樣就要每個各自設定一個 Route，=> 改用 Attribute Routing

			app.MapRazorPages();

			app.Run();
		}
	}
}
