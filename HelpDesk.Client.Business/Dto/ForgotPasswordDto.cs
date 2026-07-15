using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Client.Dto.Documents
{
	public class ForgotPasswordDto
	{
		[EmailAddress]
		[Required(ErrorMessage = "Email is required")]
		[StringLength(100, MinimumLength = 7)]
		public string Email { get; set; } = string.Empty;
	}
}
