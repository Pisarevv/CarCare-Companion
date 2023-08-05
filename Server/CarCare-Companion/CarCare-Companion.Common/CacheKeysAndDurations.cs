namespace CarCare_Companion.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class CacheKeysAndDurations
{
    public static class CarouselAds
    {
        public static string CarouselAdsCacheKey = "CarouselAds";
        public static int CarouselAdsCacheDurationMinutes = 10;
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
        public static int UserTripsCostDurationMinutes = 10;

        public static string UserTripsCountCacheKeyAddition = "UserTripsCount";
        public static int UserTripsCountDurationMinutes = 10;
    }


}
