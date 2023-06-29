﻿// <auto-generated />
using System;
using CarCare_Companion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarCare_Companion.Infrastructure.Migrations
{
    [DbContext(typeof(CarCareCompanionDbContext))]
    partial class CarCareCompanionDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CarCare_Companion.Infrastructure.Data.Models.Ads.CarouselAdModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Identifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Date of creation");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Date of deleting");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasComment("Deleted status");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Last date of modification");

                    b.Property<int>("StarsRating")
                        .HasColumnType("int");

                    b.Property<string>("UserFirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("CarouselAdModels");

                    b.HasData(
                        new
                        {
                            Id = new Guid("34821551-eb2c-4765-824e-213d6129b9e1"),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "The car maintenance and service management website is user-friendly, offers a wide service network, effective communication, and robust record-keeping capabilities for a seamless experience.",
                            IsDeleted = false,
                            StarsRating = 5,
                            UserFirstName = "David"
                        },
                        new
                        {
                            Id = new Guid("9d5fed18-a130-4bef-9d83-a784d73fb96c"),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "With its intuitive interface, extensive service network, detailed record-keeping the car maintenance and service management website ensures a top-notch experience for all users.",
                            IsDeleted = false,
                            StarsRating = 5,
                            UserFirstName = "Peter"
                        },
                        new
                        {
                            Id = new Guid("ab45c5fd-f288-4aa2-907b-84800ae06028"),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Experience exceptional car maintenance and service management with a user-friendly website interface, comprehensive service network, efficient communication, thorough record-keeping, and prompt notifications for a hassle-free experience.",
                            IsDeleted = false,
                            StarsRating = 5,
                            UserFirstName = "Michael"
                        },
                        new
                        {
                            Id = new Guid("5f7f248a-a136-4079-acad-5285e08bce4a"),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "The forum within the website fosters a vibrant community, enabling car owners to communicate, share valuable information, and seek advice for a collaborative and informative experience.",
                            IsDeleted = false,
                            StarsRating = 5,
                            UserFirstName = "Bob"
                        },
                        new
                        {
                            Id = new Guid("186d3c86-41fb-41a6-be74-a12f6004894f"),
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "The website's interactive forum creates a dynamic space where car enthusiasts can connect, exchange valuable insights, and foster a supportive community for engaging discussions and information sharing.",
                            IsDeleted = false,
                            StarsRating = 5,
                            UserFirstName = "Paul"
                        });
                });

            modelBuilder.Entity("CarCare_Companion.Infrastructure.Data.Models.Identity.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("CarCare_Companion.Infrastructure.Data.Models.Identity.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Date of user creation");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("User first name");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("User last name");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Last date of user modification");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ProfileImageKey")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Image key for user profile picture");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("CarCare_Companion.Infrastructure.Data.Models.Records.ServiceRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Identifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Date of creation");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Date of deleting");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Description of the service");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasComment("Deleted status");

                    b.Property<double>("Mileage")
                        .HasColumnType("float")
                        .HasComment("Vehicle mileage");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Last date of modification");

                    b.Property<DateTime>("PerformedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Date of the performed service");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("The title of the record");

                    b.Property<Guid>("VehicleId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("The vehicle identifier");

                    b.HasKey("Id");

                    b.HasIndex("VehicleId");

                    b.ToTable("ServiceRecords");
                });

            modelBuilder.Entity("CarCare_Companion.Infrastructure.Data.Models.Records.TripRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Identifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Date of creation");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Date of deleting");

                    b.Property<string>("EndDestination")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("The end destination of the trip");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasComment("Deleted status");

                    b.Property<double>("MileageTravelled")
                        .HasColumnType("float")
                        .HasComment("The travelled destination on the trip");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Last date of modification");

                    b.Property<string>("StartDestination")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("The start destination of the trip");

                    b.Property<double?>("UsedFuel")
                        .HasColumnType("float")
                        .HasComment("The used destination on the trip");

                    b.Property<Guid>("VehicleId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("The vehicle identifier");

                    b.HasKey("Id");

                    b.HasIndex("VehicleId");

                    b.ToTable("TripRecords");
                });

            modelBuilder.Entity("CarCare_Companion.Infrastructure.Data.Models.Vehicle.FuelType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Fuel id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasComment("Name of the fuel");

                    b.HasKey("Id");

                    b.ToTable("FuelTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Petrol"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Diesel"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Electric"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Hybrid"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Petrol/LPG"
                        });
                });

            modelBuilder.Entity("CarCare_Companion.Infrastructure.Data.Models.Vehicle.Vehicle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Identifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Date of creation");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Date of deleting");

                    b.Property<int>("FuelTypeId")
                        .HasColumnType("int")
                        .HasComment("Used fuel type identifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasComment("Deleted status");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Make name");

                    b.Property<double>("Mileage")
                        .HasColumnType("float")
                        .HasComment("Mileage of the vehicle");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Model name");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Last date of modification");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Vehicle owner identifier");

                    b.Property<Guid?>("VehicleImageKey")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Image key for vehicle picture");

                    b.Property<int?>("VehicleTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FuelTypeId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("CarCare_Companion.Infrastructure.Data.Models.Vehicle.VehicleType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Type id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasComment("Name of the type");

                    b.HasKey("Id");

                    b.ToTable("VehicleTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Sedan"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Hatchback"
                        },
                        new
                        {
                            Id = 3,
                            Name = "SUV"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Crossover"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Coupe"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Convertible"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Minivan"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Pickup Truck"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Van"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Truck"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CarCare_Companion.Infrastructure.Data.Models.Records.ServiceRecord", b =>
                {
                    b.HasOne("CarCare_Companion.Infrastructure.Data.Models.Vehicle.Vehicle", "Vehicle")
                        .WithMany("ServiceRecords")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("CarCare_Companion.Infrastructure.Data.Models.Records.TripRecord", b =>
                {
                    b.HasOne("CarCare_Companion.Infrastructure.Data.Models.Vehicle.Vehicle", "Vehicle")
                        .WithMany("TripRecords")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("CarCare_Companion.Infrastructure.Data.Models.Vehicle.Vehicle", b =>
                {
                    b.HasOne("CarCare_Companion.Infrastructure.Data.Models.Vehicle.FuelType", "FuelType")
                        .WithMany("Vehicles")
                        .HasForeignKey("FuelTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CarCare_Companion.Infrastructure.Data.Models.Identity.ApplicationUser", "Owner")
                        .WithMany("Vehicles")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CarCare_Companion.Infrastructure.Data.Models.Vehicle.VehicleType", null)
                        .WithMany("Vehicles")
                        .HasForeignKey("VehicleTypeId");

                    b.Navigation("FuelType");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("CarCare_Companion.Infrastructure.Data.Models.Identity.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("CarCare_Companion.Infrastructure.Data.Models.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("CarCare_Companion.Infrastructure.Data.Models.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("CarCare_Companion.Infrastructure.Data.Models.Identity.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarCare_Companion.Infrastructure.Data.Models.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("CarCare_Companion.Infrastructure.Data.Models.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CarCare_Companion.Infrastructure.Data.Models.Identity.ApplicationUser", b =>
                {
                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("CarCare_Companion.Infrastructure.Data.Models.Vehicle.FuelType", b =>
                {
                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("CarCare_Companion.Infrastructure.Data.Models.Vehicle.Vehicle", b =>
                {
                    b.Navigation("ServiceRecords");

                    b.Navigation("TripRecords");
                });

            modelBuilder.Entity("CarCare_Companion.Infrastructure.Data.Models.Vehicle.VehicleType", b =>
                {
                    b.Navigation("Vehicles");
                });
#pragma warning restore 612, 618
        }
    }
}
