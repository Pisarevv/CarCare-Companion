namespace CarCare_Companion.Infrastructure.Data.Models.Identity;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

using CarCare_Companion.Infrastructure.Data.Models.Vehicle;

using static CarCare_Companion.Common.ValidationConstants.ApplicationUser;

public class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser()
    {
       this.Vehicles = new HashSet<Vehicle>(); 
    }

    [MinLength(MinFirstNameLength)]
    [MaxLength(MaxFirstNameLength)]
    public string? FirstName { get; set; }

    [MinLength(MinLastNameLength)]
    [MaxLength(MaxLastNameLength)]
    public string? LastName { get; set;}

    public ICollection<Vehicle> Vehicles { get; set; }

}
