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
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));

			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			builder.Services.AddDbContext<NorthwindContext>(options => { 
			options.UseSqlServer(builder.Configuration.GetConnectionString("Northwind"));
			});

			builder.Services.AddSession(options =>
			{
				options.Cookie.Name = ".CustomerWebSite.Session";
				options.IdleTimeout = TimeSpan.FromMinutes(5);
				options.Cookie.IsEssential = true;
				options.Cookie.HttpOnly = true;
				options.Cookie.SecurePolicy=CookieSecurePolicy.Always;
			});

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

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseSession();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapRazorPages();

			app.Run();
		}
	}
}
