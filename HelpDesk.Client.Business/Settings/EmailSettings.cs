namespace HelpDesk.Client.Business.Settings
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int PortNumber { get; set; }
        public string EmailSender { get; set; }
        public string EmailSenderPassword { get; set; }
        public bool IsSSL { get; set; }
        public string TemplatesPath { get; set; }
    }
}
