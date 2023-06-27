namespace CarCare_Companion.Core.Models.Status;

public class StatusErrorInformation
{
    public StatusErrorInformation(string input)
    {
        this.Title = input;
    }
    public string Title { get; } = null!;
}
