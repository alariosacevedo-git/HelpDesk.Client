namespace HelpDesk.Client.Business.Services
{
    public interface IEmailService
    {
        Task<bool> EmailConfirmation(string email, string token, string confirmationLink);
        Task<bool> ResetPassword(string email, string token, string confirmationLink);
    }
}
