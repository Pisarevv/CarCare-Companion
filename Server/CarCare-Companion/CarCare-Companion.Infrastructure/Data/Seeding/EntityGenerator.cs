using CarCare_Companion.Infrastructure.Data.Models.Ads;
using CarCare_Companion.Infrastructure.Data.Models.Vehicle;

namespace CarCare_Companion.Infrastructure.Data.Seeding;

public static class EntityGenerator 
{
    public static ICollection<FuelType> GenerateFuelTypes()
    {
        return new HashSet<FuelType>()
        {
            new FuelType()
            {
                Id = 1,
                Name = "Petrol"
            },
             new FuelType()
            {
                Id = 2,
                Name = "Diesel"
            },
              new FuelType()
            {
                Id = 3,
                Name = "Electric"
            },
              new FuelType()
            {
                Id = 4,
                Name = "Hybrid"
            },
              new FuelType()
            {
                Id = 5,
                Name = "Petrol/LPG"
            }

        };
    }

    public static ICollection<VehicleType> GenerateVehicleTypes()
    {
        return new HashSet<VehicleType>()
        {
            new VehicleType
            { 
                Id = 1, 
                Name = "Sedan"
            },
            new VehicleType
            {
                Id = 2, 
                Name = "Hatchback"
            },
            new VehicleType 
            { 
                Id = 3,
                Name = "SUV"
            },
            new VehicleType 
            {
                Id = 4, 
                Name = "Crossover" 
            },
            new VehicleType 
            { 
                Id = 5, 
                Name = "Coupe"
            },
            new VehicleType 
            { 
                Id = 6, 
                Name = "Convertible" 
            },
            new VehicleType 
            { 
                Id = 7,
                Name = "Minivan"
            },
            new VehicleType 
            { 
                Id = 8,
                Name = "Pickup Truck"
            },
            new VehicleType 
            { 
                Id = 9,
                Name = "Van" 
            },
            new VehicleType 
            { 
                Id = 10, 
                Name = "Truck"
            },
           
        };
    }

    public static ICollection<CarouselAdModel> GenerateCarouselAdModels()
    {
        return new HashSet<CarouselAdModel>()
        {
            new CarouselAdModel
            {
                Id = Guid.NewGuid(),
                UserFirstName = "David",
                Description = "The car maintenance and service management website is user-friendly, offers a wide service network, effective communication, and robust record-keeping capabilities for a seamless experience.",
                StarsRating = 5,
            },

             new CarouselAdModel
            {
                Id = Guid.NewGuid(),
                UserFirstName = "Peter",
                Description = "With its intuitive interface, extensive service network, detailed record-keeping the car maintenance and service management website ensures a top-notch experience for all users.",
                StarsRating = 5,
            },
              new CarouselAdModel
            {
                Id = Guid.NewGuid(),
                UserFirstName = "Michael",
                Description = "Experience exceptional car maintenance and service management with a user-friendly website interface, comprehensive service network, efficient communication, thorough record-keeping, and prompt notifications for a hassle-free experience.",
                StarsRating = 5,
            },
              new CarouselAdModel
            {
                Id = Guid.NewGuid(),
                UserFirstName = "Bob",
                Description = "The forum within the website fosters a vibrant community, enabling car owners to communicate, share valuable information, and seek advice for a collaborative and informative experience.",
                StarsRating = 5,
            },
               new CarouselAdModel
            {
                Id = Guid.NewGuid(),
                UserFirstName = "Paul",
                Description = "The website's interactive forum creates a dynamic space where car enthusiasts can connect, exchange valuable insights, and foster a supportive community for engaging discussions and information sharing.",
                StarsRating = 5,
            },



        };
    }
}
