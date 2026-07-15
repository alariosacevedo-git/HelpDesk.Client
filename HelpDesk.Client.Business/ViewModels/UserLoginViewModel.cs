using HelpDesk.Client.Dto.Documents;

namespace HelpDesk.Client.Business.ViewModels
{
    public class UserLoginViewModel
    {
        public UserLoginViewModel()
        {
            UserLogin = new UserLoginDto();
            UserDto = new UserDto();
        }

        public UserLoginDto UserLogin { get; set; }

        public UserDto? UserDto { get; set; }
    }
}
