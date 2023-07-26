namespace CarCare_Companion.Infrastructure.Data.Models.Identity;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserRefreshToken
{
    [Key]
    public Guid Id { get; set; }

    [Comment("The user refresh token")]
    public string? RefreshToken { get; set; }

    [Comment("The refresh token expiration date")]
    public DateTime? RefreshTokenExpiration { get; set; }

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
 
    public ApplicationUser User { get; set; } = null!;
}
