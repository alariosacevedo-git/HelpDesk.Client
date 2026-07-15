using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Client.Dto.Documents
{
	public class UserLoginDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = "Colima1982@";


		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}
}
