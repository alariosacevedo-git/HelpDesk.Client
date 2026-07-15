using HelpDesk.Client.Dto.Documents;
using Microsoft.AspNetCore.Identity;

namespace HelpDesk.Client.Business.ViewModels
{
    public class UserRegistrationViewModel
    {
        public UserRegistrationViewModel()
        {
            UserRegistration = new UserRegistrationDto();
            IdentityResult = new IdentityResult();
        }

        public UserRegistrationDto UserRegistration { get; set; }
        public IdentityResult? IdentityResult { get; set; }
        public bool IsUserCreated { get; set; } = false;
        public string? HttpClientError { get; set; } = string.Empty;

    }
}
