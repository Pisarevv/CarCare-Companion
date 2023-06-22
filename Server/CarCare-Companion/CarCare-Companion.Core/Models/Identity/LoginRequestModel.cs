namespace CarCare_Companion.Core.Models.Identity;

using System.ComponentModel.DataAnnotations;

public class LoginRequestModel
{

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

}
