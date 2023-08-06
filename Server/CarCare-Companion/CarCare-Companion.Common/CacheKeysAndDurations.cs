namespace CarCare_Companion.Common;


public static class CacheKeysAndDurations
{
    public static class CarouselAds
    {
        public static string CarouselAdsCacheKey = "CarouselAds";
        public static int CarouselAdsCacheDurationMinutes = 10;
    }

    public static class Users
    {
        public static string UsersCacheKey = "Users";
        public static int UsersCacheDurationMinutes = 2;
    }

    public static class Vehicles
    {
        public static string UserVehiclesCacheKeyAddition = "UserVehicles";
        public static int UserVehiclesCacheDurationMinutes = 10;

        public static string VehicleDetailsCacheKeyAddition = "VehicleDetails";
        public static int VehicleDetailCacheDurationMinutes = 10;

        public static string FuelTypesCacheKey = "FuelTypes";
        public static int FuelTypesCacheDurationMinutes = 10;

        public static string VehicleTypesCacheKey = "VehicleTypes";
        public static int VehicleTypesCacheDurationMinutes = 10;
    }

    public static class Trips
    {
        public static string UserTripsCacheKeyAddition = "UserTrips";
        public static int UserTripsCacheDurationMinutes = 10;

        public static string UserTripsCostCacheKeyAddition = "UserTripsCost";
        public static int UserTripsCostCacheDurationMinutes = 10;

        public static string UserTripsCountCacheKeyAddition = "UserTripsCount";
        public static int UserTripsCountCacheDurationMinutes = 10;
    }

    public static class TaxRecords
    {
        public static string UserTaxRecordsCacheKeyAddition = "UserTaxRecords";
        public static int UserTaxRecordsCacheDuration = 5;

        public static string UserTaxRecordsCountCacheKeyAddition = "UserTaxRecordsCount";
        public static int UserTaxRecordsCountCacheDuration = 10;

        public static string UserTaxRecordsCostCacheKeyAddition = "UserTaxRecordsCost";
        public static int UserTaxRecordsCostCacheDuration = 10;

        public static string UserTaxRecordsUpcomingCacheKeyAddition = "UserTaxRecordsUpcoming";
        public static int UserTaxRecordsUpcomingCacheDuration = 10;
    }

    public static class ServiceRecords
    {
        public static string UserServiceRecordsCacheKeyAddition = "UserServiceRecords";
        public static int UserServiceRecordsCacheDuration = 5;

        public static string UserServiceRecordsCountCacheKeyAddition = "UserServiceRecordsCount";
        public static int UserServiceRecordsCountCacheDuration = 10;

        public static string UserServiceRecordsCostCacheKeyAddition = "UserServiceRecordsCost";
        public static int UserServiceRecordsCostCacheDuration = 10;

        public static string UserServiceRecordsLastNCacheKeyAddition = "UserServiceRecordsLastN";
        public static int UserServiceRecordsLastNCacheDuration = 10;
    }


}
