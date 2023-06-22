namespace CarCare_Companion.Core.Models.Identity;

using System.ComponentModel.DataAnnotations;

using static CarCare_Companion.Common.ValidationConstants.ApplicationUser;

public class RegisterRequestModel
{

    [Required]
    [MinLength(MinFirstNameLength)]
    [MaxLength(MaxFirstNameLength)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MinLength(MinLastNameLength)]
    [MaxLength(MaxLastNameLength)]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = null!;

}
