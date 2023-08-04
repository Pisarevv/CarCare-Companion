namespace CarCare_Companion.Api.Jobs;

using Microsoft.Extensions.Logging;

using Quartz;

using CarCare_Companion.Core.Contracts;


public class SendUsersTaxReminder : IJob
{
    private readonly ILogger<SendUsersTaxReminder> logger;
    private readonly IMessagingService messagingService;

    public SendUsersTaxReminder(ILogger<SendUsersTaxReminder> logger, IMessagingService messagingService)
    {
        this.logger = logger;
        this.messagingService = messagingService;
    }

    public async Task Execute(IJobExecutionContext context)
    { 
        logger.LogInformation($"Starting executing tax reminder messaging job at {DateTime.Now}");
        try
        {
            await messagingService.SendTaxReminderEmailsToUsers();
            logger.LogInformation($"Executed tax reminder messaging job at {DateTime.Now}");
        }
        catch (Exception ex)
        {
            logger.LogError($"Executing tax reminder messaging job failed at {DateTime.Now}", ex.Message);
        }

        return;
    }
}
