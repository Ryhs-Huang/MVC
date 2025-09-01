using System.ComponentModel.DataAnnotations;

namespace CustomerWebSite.ViewModels
{
	public class ContactViewModel : IValidatableObject
	{
		[Display(Name = "姓名")]
		[Required(ErrorMessage = "姓名為必填欄位")]
		[StringLength(maximumLength: 8, MinimumLength = 3, ErrorMessage = "姓名最少需要3個字元")]
		public string Name { get; set; }

		[Display(Name = "電子郵件")]
		[EmailAddress(ErrorMessage = "電子郵件格式錯誤")]
		public string? Email { get; set; }

		[Display(Name = "連絡電話")]
		public string? Phone { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Phone))
			{
				yield return new ValidationResult("姓名和電話至少要填寫一個");
			}
		}
	}
}