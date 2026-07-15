using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Client.Dto.Documents
{
    public class UserRegistrationDto
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }


        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, MinimumLength = 7)]
        public string Email { get; set; }

        [Phone]
        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(20, MinimumLength = 14)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 3)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [StringLength(20, MinimumLength = 3)]
        public string ConfirmPassword { get; set; }
    }
}
