namespace CarCare_Companion.Common;
public static class GlobalConstants
{
    public const string AdministratorRoleName = "Administrator";

    //Minutes
    public const double JWTTokenExpirationTime = 15;

    //Days
    public const double RefreshTokenExpirationTime = 10;

    public const string AWSBucket = "car-care-companion-bucket";

    public const int RecentlyJoinedUsersDaysLookback = 7;

}
