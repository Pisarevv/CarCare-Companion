namespace CarCare_Companion.Infrastructure.Data.Models.Identity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserRefreshToken
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string RefreshToken { get; set; } = null!;

    [Required]
    public DateTime RefreshTokenExpiration { get; set; }

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;
}
