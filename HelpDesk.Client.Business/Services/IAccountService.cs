using HelpDesk.Client.Business.ViewModels;
using HelpDesk.Client.Dto.Documents;
using Microsoft.AspNetCore.Identity;

namespace HelpDesk.Client.Business.Services
{
    public interface IAccountService
    {
        Task<UserDto> Login(UserLoginDto userLogin);
        Task<IdentityResult> Create(UserRegistrationViewModel model);
        Task<UserDto> FindUserByEmail(string Email);
        Task<string> GenerateEmailConfirmationToken(UserDto userDto);
        Task<UserRegisteredDto> Register(UserRegistrationDto userRegistration);
        Task<bool> EmailConfirmed(EmailConfirmedDto emailConfirmed);
        Task<string> GeneratePasswordResetToken(UserDto userDto);
    }
}
