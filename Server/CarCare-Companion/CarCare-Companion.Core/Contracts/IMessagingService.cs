
namespace CarCare_Companion.Core.Contracts;

using System.Threading.Tasks;

public interface IMessagingService
{
    /// <summary>
    /// Sends tax reminding emails to users.
    /// </summary>
    public Task SendTaxReminderEmailsToUsers();
}
