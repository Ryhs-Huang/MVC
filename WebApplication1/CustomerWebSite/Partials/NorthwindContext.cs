using Microsoft.EntityFrameworkCore;

namespace CustomerWebSite.Models
{
	public partial class NorthwindContext : DbContext
	{
		public NorthwindContext() { }//建構子
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//EF Core 提供的一個可覆寫方法。你可以在這裡指定「要連線到哪個資料庫」，也可以設定連線字串、資料庫類型等。
		{
			if (!optionsBuilder.IsConfigured)//如果這個 DbContext 還沒有被設定過
			{
				IConfiguration Config = new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile("appsettings.json")
				.Build();//把前面所有的設定來源（此處是 appsettings.json）建立成一個可以查詢的設定物件，也就是 IConfiguration 實體。
				//載入 設定檔 appsettings.json，裡面會包含你的資料庫連線字串
				optionsBuilder.UseSqlServer(Config.GetConnectionString("Northwind"));
				//告訴 EF Core 使用 SQL Server，並從設定檔中找 "Northwind" 這個連線字串
			}
		}
	}
}
