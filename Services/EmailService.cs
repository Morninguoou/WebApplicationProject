using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using marian_onsite.Models;

namespace marian_onsite.Services;

public class EmailService
{
    private readonly IOptions<EmailSettings> _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings;
    }
    
    public async Task SendEmailAsync(string email, string subject, string body)
    {
        string smtpServer = _emailSettings.Value.SmtpServer;
        int port = _emailSettings.Value.SmtpPort;
        string senderEmail = _emailSettings.Value.SenderEmail;
        string senderPassword = _emailSettings.Value.SenderPassword;

        if (!IsValidEmail(senderEmail))
            throw new ArgumentException("Invalid sender email address");

        if (string.IsNullOrEmpty(senderPassword))
            throw new ArgumentException("Sender password is required");

        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid recipient email address");

        using (SmtpClient client = new SmtpClient(smtpServer, port))
        {
            client.EnableSsl = true;

            client.Credentials = new NetworkCredential(new MailAddress(senderEmail).Address, senderPassword);

            using (MailMessage message = new MailMessage(new MailAddress(senderEmail), new MailAddress(email)))
            {
                message.Subject = subject;
                message.Body = body;

                try
                {
                    await client.SendMailAsync(message);
                }
                catch (Exception ex)
                {
                    // Handle exception
                    throw new Exception(ex.Message);
                }
            }
        }
    }


    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}