namespace CarCare_Companion.Core.Models.Identity;

public class AuthDataInternalTransferModel
{
    public string AccessToken { init; get; } = null!;

    public string RefreshToken { init; get; } = null!;

    public string Email { init; get; } = null!;

    public string Role { init; get; } = null!;
}
