using HelpDesk.Client.Business.Observers;
using Microsoft.AspNetCore.Identity;

namespace HelpDesk.Client.Dto.Documents
{
    public class UserDto : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
