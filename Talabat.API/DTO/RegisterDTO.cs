using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTO
{
	public class RegisterDTO
	{
		[Required]

		public string DisplayName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		[Phone]
		public string PhoneNumber { get; set; }
		[RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{8,}$")]
		public string Password { get; set; }

	}
}
