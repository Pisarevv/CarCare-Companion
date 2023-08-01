namespace CarCare_Companion.Core.Models.Admin.Users;

public class UserDetailsResponseModel
{
    public string UserId { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? FirstName { get; set; } = null!;

    public string? LastName { get; set; } = null!;

    public int VehiclesCount { get; set; }

    public int ServiceRecordsCount { get; set; }

    public int TaxRecordsCount { get; set; }

    public int TripsCount { get; set; }

    public bool IsAdmin { get; set; }

}
