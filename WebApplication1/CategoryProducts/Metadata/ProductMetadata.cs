using System.ComponentModel.DataAnnotations;

namespace CategoryProducts.Metadata
{
	internal class ProductMetadata
	{
		[Display(Name ="商品名稱")]
		[StringLength(maximumLength:40, MinimumLength =8, ErrorMessage ="商品名稱至少8個字")]
		public string ProductName { get; set; } = null!;

		[DisplayFormat(DataFormatString ="{0:c}")]
		[Display(Name = "商品單價")]
		public decimal? UnitPrice { get; set; }

		[Display(Name ="訂購數量")]
		[Range(1,100,ErrorMessage ="訂購數量最少1個，最多100個")]
		public short? UnitsOnOrder { get; set; }


	}
}