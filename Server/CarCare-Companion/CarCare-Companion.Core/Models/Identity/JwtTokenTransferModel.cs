namespace CarCare_Companion.Core.Models.Identity;

public class JwtTokenTransferModel
{
    public string Token { init; get; } = null!;

    public DateTime? Expiration { init; get; }
}
