namespace CarCare_Companion.Core.Messaging;

using CarCare_Companion.Api.Extensions.Mail;
using CarCare_Companion.Core.Messaging.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

public class GmailEmailSender : IEmailSender
{
    private readonly MailSettings mailSettings;
    public GmailEmailSender(IOptions<MailSettings> mailSettings) 
    {
        this.mailSettings = mailSettings.Value;
    }

    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(mailSettings.Mail);
        email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
        email.Subject = mailRequest.Subject;
        var builder = new BodyBuilder();
       
        builder.HtmlBody = mailRequest.Body;
        email.Body = builder.ToMessageBody();
        using (var smtp = new SmtpClient())
        {
            smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
          
    }

    public async Task SendEmailsAsync(ICollection<MailRequest> mailRequests)
    {
        var emails = new List<MimeMessage>();

        foreach (var mailRequest in mailRequests)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();

            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            emails.Add(email);
        }

        using (var smtp = new SmtpClient())
        {
            smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
            foreach(var email in emails)
            {
                smtp.Send(email);
            }
            smtp.Disconnect(true);
        }
    }
}
