namespace CarCare_Companion.Core.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Messaging;
using CarCare_Companion.Core.Messaging.Models;
using CarCare_Companion.Core.Models.TaxRecords;

using static Messaging.EmailTemplates.TemplatePaths;


public class MessagingService : IMessagingService
{
    private readonly ITaxRecordsService taxRecordsService;
    private readonly IEmailSender emailSender;
    private readonly ILogger<MessagingService> logger;

    public MessagingService(ITaxRecordsService taxRecordsService, IEmailSender emailSender, ILogger<MessagingService> logger)
    {
        this.taxRecordsService = taxRecordsService;
        this.emailSender = emailSender;
        this.logger = logger;
    }

    public async Task SendTaxReminderEmailsToUsers()
    {
        ICollection<MailRequest> mailRequests = new List<MailRequest>();

        ICollection<UpcomingUserTaxResponseModel> upcomingUserTaxes = await taxRecordsService.GetUpcomingUsersTaxesAsync();

        if(upcomingUserTaxes.Count == 0)
        {
            return;
        }

        string templateBody = await RetrieveEmailTemplate(TaxRecordExpiryNoticePath);

        foreach (var upcomingUserTax in  upcomingUserTaxes)
        {
            string formattedBody = FillTaxReminderBody(templateBody, upcomingUserTax);

            MailRequest mailRequest = new MailRequest()
            {
                ToEmail = upcomingUserTax.Email,
                Subject = $"Upcoming {upcomingUserTax.TaxName} on your {upcomingUserTax.VehicleMake} {upcomingUserTax.VehicleModel}",
                Body = formattedBody
            };

            mailRequests.Add(mailRequest);
        }

        await emailSender.SendEmailsAsync(mailRequests);
    
    }

    private async Task<string> RetrieveEmailTemplate(string relativePathToTemplate)
    {
        string rootPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
        var templateLocation = Path.Combine(rootPath, relativePathToTemplate);
        templateLocation = Path.GetFullPath(templateLocation);

        if (!File.Exists(templateLocation))
        {
            throw new FileNotFoundException("Email template not found.");
        }

        string templateBody = await File.ReadAllTextAsync(templateLocation);

        return templateBody;
    }

    private string FillTaxReminderBody(string templateBody, UpcomingUserTaxResponseModel upcomingUserTax)
    {
        string formattedBody = templateBody;
        formattedBody = formattedBody.Replace("[First Name]", upcomingUserTax.FirstName);
        formattedBody = formattedBody.Replace("[Last Name]", upcomingUserTax.LastName);
        formattedBody = formattedBody.Replace("[Tax Name]", upcomingUserTax.TaxName);
        formattedBody = formattedBody.Replace("[Expiration Date]", upcomingUserTax.TaxValidTo);

        return formattedBody;
    }
}
