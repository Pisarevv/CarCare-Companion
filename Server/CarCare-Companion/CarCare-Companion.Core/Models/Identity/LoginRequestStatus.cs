namespace CarCare_Companion.Core.Models.Identity;

public class LoginRequestStatus
{
    public bool LoginSuccessful{ init; get; }

    public string? Token { init; get; }

    public DateTime? Expiration { init; get; }

    public string StatusMassage { init; get; } = null!;
}
