using System.ComponentModel.DataAnnotations;

namespace CategoryProducts.Metadata
{
	internal class ProductMetadataType
	{
		// 顯示規則
		[Display(Name = "商品名稱")]
		// 驗證規則
		[StringLength(maximumLength: 40, MinimumLength = 8, ErrorMessage = "商品名稱至少要8個字")]
		// 因為資料表就是設計最長40
		[Required(ErrorMessage = "商品名稱未填寫")] // 因為資料表設定為 NOT NULL
		public string ProductName { get; set; } = null!;

		[DisplayFormat(DataFormatString = "{0:C2}")]
		[Display(Name = "商品單價")]
		public decimal? UnitPrice { get; set; }

		[Display(Name = "訂購數量")]
		[Range(1, 100, ErrorMessage = "訂購數量必須介於1~100之間")]
		public short? UnitsOnOrder { get; set; }
	}
}