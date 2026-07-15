using Microsoft.AspNetCore.Identity;

namespace HelpDesk.Client.Dto.Documents
{
    public class UserRegisteredDto
    {
        public UserRegisteredDto()
        {
            User = new UserDto();
            IdentityResult = new IdentityResult();
        }

        public UserDto User { get; set; }

        public IdentityResult IdentityResult { get; set; }

        public string Token { get; set; }
    }
}
