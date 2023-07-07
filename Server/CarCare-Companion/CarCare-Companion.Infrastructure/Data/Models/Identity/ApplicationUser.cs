namespace CarCare_Companion.Infrastructure.Data.Models.Identity;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

using CarCare_Companion.Infrastructure.Data.Models.Contracts;
using CarCare_Companion.Infrastructure.Data.Models.Vehicle;

using static CarCare_Companion.Common.ValidationConstants.ApplicationUser;
using Microsoft.EntityFrameworkCore;
using CarCare_Companion.Infrastructure.Data.Models.Records;

public class ApplicationUser : IdentityUser<Guid>, IAuditInfo
{
    public ApplicationUser()
    {
       this.Vehicles = new HashSet<Vehicle>(); 
       this.TripRecords = new HashSet<TripRecord>();
       this.ServiceRecords = new HashSet<ServiceRecord>();
       this.TaxRecords = new HashSet<TaxRecord>();
    }

    [Comment("User first name")]
    [MinLength(MinFirstNameLength)]
    [MaxLength(MaxFirstNameLength)]
    public string? FirstName { get; set; }

    [Comment("User last name")]
    [MinLength(MinLastNameLength)]
    [MaxLength(MaxLastNameLength)]
    public string? LastName { get; set;}

    [Comment("Date of user creation")]
    public DateTime CreatedOn { get; set; }

    [Comment("Last date of user modification")]
    public DateTime? ModifiedOn { get; set ; }

    [Comment("Image key for user profile picture")]
    public Guid? ProfileImageKey { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; }

    public ICollection<TripRecord> TripRecords { get; set; }

    public ICollection<ServiceRecord> ServiceRecords { get; set; }

    public ICollection<TaxRecord> TaxRecords { get; set; }
}
