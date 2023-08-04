using Quartz;

using CarCare_Companion.Api.Extensions.Quartz;
using CarCare_Companion.Api.Jobs;

using static CarCare_Companion.Common.GlobalConstants;

namespace CarCare_Companion.Api.Extensions.Quartz;

public static class QuartzConfigurator
{
    public static void AddQuartz(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionScopedJobFactory();

            // Create a "key" for the job
            var jobKey = new JobKey("SendUsersTaxReminder");

            // Register the job with the DI container
            q.AddJob<SendUsersTaxReminder>(opts => opts.WithIdentity(jobKey));

            // Create a trigger for the job
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("MyJobTrigger")
                .WithCronSchedule(SendUsersTaxReminderSchedule)); 
        });

        services.AddQuartzHostedService(
        q => q.WaitForJobsToComplete = true
        );
    }
}
