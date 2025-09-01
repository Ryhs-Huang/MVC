using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthWebSite.Data
{
	// ApplicationUser 是 Model
	public class ApplicationUser : IdentityUser
	{
		[MaxLength(3)]  // 沒加會nvarchar(max)
		public string Country { get; set; }  


	}
}
