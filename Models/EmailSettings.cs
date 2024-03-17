namespace marian_onsite.Models
{
    public class EmailSettings : IEmailSettings
    {
        public string SmtpServer { get; set; } = null!;
        public int SmtpPort { get; set; }
        public string SenderEmail { get; set; } = null!;
        public string SenderPassword { get; set; } = null!;
    }

    public interface IEmailSettings
    {
        string SmtpServer { get; set; }
        int SmtpPort { get; set; }
        string SenderEmail { get; set; }
        string SenderPassword { get; set; }
    }
}