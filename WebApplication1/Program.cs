using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the DI container.
			// 註冊服務到 DI (Dependency Injection) 容器中
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.AddControllersWithViews();

			var app = builder.Build();

			//Configure the HTTP request pipeline.
			//控制錯誤訊息的顯示
			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
			}
			else
			{
				// /Home下沒有 Error.cshtml
				// 如果找不到該畫面，就會去 share 資料夾找
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			// Request pipeline
			// HTTP 要求管線 (明管配置)
			app.UseHttpsRedirection();  // user 如果瀏覽到 HTTP 網址，會自動重導到 HTTPS 網址(加密連線)，保護 user 的隱私
			app.UseStaticFiles();       // 指定存放網站靜態文件的預設資料夾為：wwwroot
			app.UseRouting();           // 執行 URL Routing / URL Rewriting，減少網址暴露過多資訊
			app.UseAuthorization();     // 啟用權限管制，(要先通過身分驗證Authenticate，才能根據身分給你權限Authorize)，檢查 user 是否有權限存取特定資源


			// 定義 Routing 規則
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");


			app.MapRazorPages();

			app.Run();
		}
	}
}
