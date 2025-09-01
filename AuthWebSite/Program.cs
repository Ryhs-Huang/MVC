using AuthWebSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//==============================
builder.Services.Configure<IdentityOptions>(options =>
{
	// 密碼設定
	options.Password.RequireDigit = true;                  // 密碼必須包含數字
	options.Password.RequireLowercase = true;              // 密碼必須包含小寫字母
	options.Password.RequireNonAlphanumeric = true;        // 密碼必須包含特殊字元（非數字與字母）
	options.Password.RequireUppercase = true;              // 密碼必須包含大寫字母
	options.Password.RequiredLength = 8;                   // 密碼至少 8 個字元
	options.Password.RequiredUniqueChars = 1;              // 至少 1 個不重複的字元

	// 鎖五分鐘 (鎖24小時不建議，太長會影響使用者體驗)
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	// 密碼錯誤 3 次就鎖定帳號
	options.Lockout.MaxFailedAccessAttempts = 3;
	// 新註冊的帳號也適用鎖定規則
	options.Lockout.AllowedForNewUsers = true;

	// 使用者名稱允許的字元集合
	options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	// 信箱必須唯一
	options.User.RequireUniqueEmail = true;
	// 登入必須先驗證過信箱
	options.SignIn.RequireConfirmedEmail = true;

});
// Cookie 設定
builder.Services.ConfigureApplicationCookie(options =>
{
	options.Cookie.HttpOnly = true;
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
	// 沒有設定ExpireTimeSpan就變成 Session Cookie，不ok
	options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
	options.LoginPath = "/Identity/Account/Login";
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";
	// 相對逾時時間
	options.SlidingExpiration = true;
});
//==============================

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

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
