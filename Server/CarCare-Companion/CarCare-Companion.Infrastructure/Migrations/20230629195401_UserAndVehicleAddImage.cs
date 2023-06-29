using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarCare_Companion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserAndVehicleAddImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("0a2790c8-9c8d-4587-bc54-4aead2aa4fea"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("1a56734e-17b2-43e7-82a1-34af1b9a6aa3"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("692f6a72-5859-40b1-a68b-433a9e07420d"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("c8efd743-e839-4372-b261-ccbf2fad48bb"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("dff1ec7a-4398-4b24-a499-6f1dabe23622"));

            migrationBuilder.AddColumn<Guid>(
                name: "VehicleImageKey",
                table: "Vehicles",
                type: "uniqueidentifier",
                nullable: true,
                comment: "Image key for vehicle picture");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileImageKey",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true,
                comment: "Image key for user profile picture");

            migrationBuilder.InsertData(
                table: "CarouselAdModels",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "Description", "IsDeleted", "ModifiedOn", "StarsRating", "UserFirstName" },
                values: new object[,]
                {
                    { new Guid("186d3c86-41fb-41a6-be74-a12f6004894f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The website's interactive forum creates a dynamic space where car enthusiasts can connect, exchange valuable insights, and foster a supportive community for engaging discussions and information sharing.", false, null, 5, "Paul" },
                    { new Guid("34821551-eb2c-4765-824e-213d6129b9e1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The car maintenance and service management website is user-friendly, offers a wide service network, effective communication, and robust record-keeping capabilities for a seamless experience.", false, null, 5, "David" },
                    { new Guid("5f7f248a-a136-4079-acad-5285e08bce4a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The forum within the website fosters a vibrant community, enabling car owners to communicate, share valuable information, and seek advice for a collaborative and informative experience.", false, null, 5, "Bob" },
                    { new Guid("9d5fed18-a130-4bef-9d83-a784d73fb96c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "With its intuitive interface, extensive service network, detailed record-keeping the car maintenance and service management website ensures a top-notch experience for all users.", false, null, 5, "Peter" },
                    { new Guid("ab45c5fd-f288-4aa2-907b-84800ae06028"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Experience exceptional car maintenance and service management with a user-friendly website interface, comprehensive service network, efficient communication, thorough record-keeping, and prompt notifications for a hassle-free experience.", false, null, 5, "Michael" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("186d3c86-41fb-41a6-be74-a12f6004894f"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("34821551-eb2c-4765-824e-213d6129b9e1"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("5f7f248a-a136-4079-acad-5285e08bce4a"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("9d5fed18-a130-4bef-9d83-a784d73fb96c"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("ab45c5fd-f288-4aa2-907b-84800ae06028"));

            migrationBuilder.DropColumn(
                name: "VehicleImageKey",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "ProfileImageKey",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "CarouselAdModels",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "Description", "IsDeleted", "ModifiedOn", "StarsRating", "UserFirstName" },
                values: new object[,]
                {
                    { new Guid("0a2790c8-9c8d-4587-bc54-4aead2aa4fea"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The forum within the website fosters a vibrant community, enabling car owners to communicate, share valuable information, and seek advice for a collaborative and informative experience.", false, null, 5, "Bob" },
                    { new Guid("1a56734e-17b2-43e7-82a1-34af1b9a6aa3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "With its intuitive interface, extensive service network, detailed record-keeping the car maintenance and service management website ensures a top-notch experience for all users.", false, null, 5, "Peter" },
                    { new Guid("692f6a72-5859-40b1-a68b-433a9e07420d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The car maintenance and service management website is user-friendly, offers a wide service network, effective communication, and robust record-keeping capabilities for a seamless experience.", false, null, 5, "David" },
                    { new Guid("c8efd743-e839-4372-b261-ccbf2fad48bb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The website's interactive forum creates a dynamic space where car enthusiasts can connect, exchange valuable insights, and foster a supportive community for engaging discussions and information sharing.", false, null, 5, "Paul" },
                    { new Guid("dff1ec7a-4398-4b24-a499-6f1dabe23622"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Experience exceptional car maintenance and service management with a user-friendly website interface, comprehensive service network, efficient communication, thorough record-keeping, and prompt notifications for a hassle-free experience.", false, null, 5, "Michael" }
                });
        }
    }
}
