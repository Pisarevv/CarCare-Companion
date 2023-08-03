namespace CarCare_Companion.Core.Messaging;

using System.Threading.Tasks;

using CarCare_Companion.Core.Messaging.Models;


public interface IEmailSender
{
    public Task SendEmailAsync(MailRequest mailRequest);

    public Task SendEmailsAsync(ICollection<MailRequest> mailRequests);
}
