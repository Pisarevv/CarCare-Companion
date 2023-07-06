namespace CarCare_Companion.Common
{
    public static class ValidationConstants
    {
        public static class ApplicationUser
        {
            public const int MinFirstNameLength = 2;
            public const int MaxFirstNameLength = 50;
            public const int MinLastNameLength = 2;
            public const int MaxLastNameLength = 50;
        }

        public static class FuelType 
        {
            public const int MinNameLength = 2;
            public const int MaxNameLength = 20;
        }

        public static class VehicleType
        {
            public const int MinNameLength = 2;
            public const int MaxNameLength = 20;
        }

        public static class ServiceRecord
        {
            public const int MinTitleLength = 2;
            public const int MaxTitleLength = 50;
            public const double MinMileage = 0;
            public const double MaxMileage = double.MaxValue;
            public const double MinCost = 0;
            public const double MaxCost = double.MaxValue;
        }


        public static class TaxRecord
        {
            public const int MinTitleLength = 2;
            public const int MaxTitleLength = 50;
            public const double MinCost = 0;
            public const double MaxCost = double.MaxValue;
        }

        public static class TripRecord
        {
            public const int MinStartDestinationNameLength = 2;
            public const int MaxStartDestinationNameLength = 100;
            public const int MinEndDestinationNameLength = 2;
            public const int MaxEndDestinationNameLength = 100;
            public const double MinTravelledRange = 0;
            public const double MaxTravelledRange = 10000;
            public const double MinUsedFuel = 0;
            public const double MaxUsedFuel = double.MaxValue;
            public const double MinFuelPrice = 0;
            public const double MaxFuelPrice = double.MaxValue;
        }

        public static class Vehicle
        {
            public const int MinMakeNameLength = 2;
            public const int MaxMakeNameLength = 50;
            public const int MinModelNameLength = 2;
            public const int MaxModelNameLength = 50;
            public const double MinMileage = 0;
            public const double MaxMileage = 999999999;
            public const int MinProductionYear = 1900;
            public const int MaxProductionYear = 2023;
        }

        public static class CarouselAdModel
        {
            public const int MinNameLength = 2;
            public const int MaxNameLength = 50;
            public const int MinDescriptionLength = 10;
            public const int MaxDescriptionLength = 500;
            public const int MinStarsRating = 0;
            public const int MaxStarsRating = 5;
        }
    }
}
