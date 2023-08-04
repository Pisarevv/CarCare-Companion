namespace CarCare_Companion.Core.Contracts;

using System.Threading.Tasks;

public interface IMessagingService
{
    public Task SendTaxReminderEmailsToUsers();
}
