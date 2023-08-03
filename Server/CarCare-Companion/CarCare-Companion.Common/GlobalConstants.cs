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


    //Quartz
    public const string SendUsersTaxReminderSchedule = "0/5 * * * * ?"; //runs every 5 seconds
    //public const string SendUsersTaxReminderSchedule = "0 0 12 ? * MON-SUN"; //runs every day of the week at 12:00pm


}
