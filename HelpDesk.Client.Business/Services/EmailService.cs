using ElmahCore;
using HelpDesk.Client.Business.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace HelpDesk.Client.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private IOptions<HelpDeskClientSettings> _helpDeskClientSettings;
        private IOptions<EmailSettings> _emailSettings;
        private string currentDirectory = string.Empty;

        public EmailService(ILogger<EmailService> logger, IOptions<HelpDeskClientSettings> helpDeskClientSettings, IOptions<EmailSettings> emailSettings)
        {
            try
            {
                _logger = logger;
                _helpDeskClientSettings = helpDeskClientSettings;
                _emailSettings = emailSettings;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                ElmahExtensions.RiseError(new Exception(ex.Message));
            }
        }

        public async Task<bool> EmailConfirmation(string email, string token, string confirmationLink)
        {
            bool IsEmailConfirmationSent = false;
            SmtpClient smtpClient = new SmtpClient();
            MailMessage mailMessage = new MailMessage();
            try
            {
                smtpClient = await GetSmtpClient(smtpClient);
                currentDirectory = Directory.GetCurrentDirectory();
                mailMessage.From = (new MailAddress(_emailSettings.Value.EmailSender, "ElDebugger", Encoding.UTF8));
                mailMessage.To.Add(new MailAddress(email, email));
                mailMessage.Subject = "Confirm Email";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = File.ReadAllText($"{currentDirectory}{_emailSettings.Value.TemplatesPath}ConfirmEmail.html");

                if (mailMessage.Body != null)
                {
                    mailMessage.Body = mailMessage.Body.ToString()
                        .Replace("<%Email%>", Convert.ToString(email))
                        .Replace("<%confirmationLink%>", Convert.ToString(confirmationLink));

                    smtpClient.Send(mailMessage);
                    IsEmailConfirmationSent = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                ElmahExtensions.RiseError(new Exception(ex.Message));
            }
            return IsEmailConfirmationSent;
        }

        public async Task<bool> ResetPassword(string email, string token, string confirmationLink)
        {
            bool IsResetPassword = false;
            try
            {
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                ElmahExtensions.RiseError(new Exception(ex.Message));
            }
            return IsResetPassword;
        }

        private async Task<SmtpClient> GetSmtpClient(SmtpClient smtpClient)
        {
            try
            {
                smtpClient.Host = _emailSettings.Value.SmtpServer;
                smtpClient.Port = _emailSettings.Value.PortNumber;
                smtpClient.EnableSsl = _emailSettings.Value.IsSSL;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_emailSettings.Value.EmailSender, _emailSettings.Value.EmailSenderPassword);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                ElmahExtensions.RiseError(new Exception(ex.Message));
            }
            return smtpClient;
        }
    }
}
