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
        public static int FuelTypesCacheDurationMinutes = 20;

        public static string VehicleTypesCacheKey = "VehicleTypes";
        public static int VehicleTypesCacheDurationMinutes = 20;
    }


}
