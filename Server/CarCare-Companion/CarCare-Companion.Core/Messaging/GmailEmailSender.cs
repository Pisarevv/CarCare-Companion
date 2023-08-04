namespace CarCare_Companion.Core.Messaging;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

using CarCare_Companion.Api.Extensions.Mail;
using CarCare_Companion.Core.Messaging.Models;


public class GmailEmailSender : IEmailSender
{
    private readonly MailSettings mailSettings;
    private readonly ILogger<GmailEmailSender> logger;

    public GmailEmailSender(IOptions<MailSettings> mailSettings, ILogger<GmailEmailSender> logger) 
    {
        this.mailSettings = mailSettings.Value;
        this.logger = logger;
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

        try
        {
            using (var smtp = new SmtpClient())
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
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

        try
        {
            using (var smtp = new SmtpClient())
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                foreach (var email in emails)
                {
                    smtp.Send(email);
                }
                smtp.Disconnect(true);
            }
        }
        catch (Exception ex)
        {

             logger.LogError(ex.Message);
        }
    }
}
