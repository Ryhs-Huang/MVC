using System.ComponentModel.DataAnnotations;

namespace CustomerWebSite.ViewModels
{
	public class ContactViewModel:IValidatableObject
	{
		[Display(Name="姓名")]
		[Required(ErrorMessage="姓名欄位不可空白")]//要求必填
		[StringLength(maximumLength:8, MinimumLength =3, ErrorMessage ="姓名至少3個字元")]
		public string Name { get; set; }

		[Display(Name = "電子郵件")]
		[EmailAddress(ErrorMessage ="電子郵件格式錯誤")]
		public string? Email { get; set; }

		[Display(Name = "連絡電話")]
		public string? Phone { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Phone))
			{
				yield return new ValidationResult("電子郵件與聯絡電話至少須擇一填寫",new string[] {"Email","Phone"});
			}
		}
	}
}