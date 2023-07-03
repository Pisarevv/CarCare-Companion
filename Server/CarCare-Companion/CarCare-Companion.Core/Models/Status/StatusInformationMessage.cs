namespace CarCare_Companion.Core.Models.Status;

public class StatusInformationMessage
{
    public StatusInformationMessage(string input)
    {
        this.Title = input;
    }
    public string Title { get; } = null!;
}
