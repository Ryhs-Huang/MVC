using CategoryProducts.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace CategoryProducts.Models
{
	[ModelMetadataType(typeof(ProductMetadataType))]
	public partial class Product
	{
	}
}
