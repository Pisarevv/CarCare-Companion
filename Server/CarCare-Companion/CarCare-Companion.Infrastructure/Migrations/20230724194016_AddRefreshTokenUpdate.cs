using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarCare_Companion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("152de202-f765-4113-b00c-aa2247a79e6d"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("3ad4564c-ac02-42cc-bdf4-5675537ce55d"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("4abb8409-e764-4233-ab9a-43d82877111b"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("69015006-2fdd-47a4-ac3f-8cc30d79f212"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("af5ddff3-f702-4beb-a498-4f287d1114ae"));

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiration",
                table: "UserRefreshToken",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "CarouselAdModels",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "Description", "IsDeleted", "ModifiedOn", "StarsRating", "UserFirstName" },
                values: new object[,]
                {
                    { new Guid("32edbfae-5b94-42b3-b6a2-191d20fbc82c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The forum within the website fosters a vibrant community, enabling car owners to communicate, share valuable information, and seek advice for a collaborative and informative experience.", false, null, 5, "Bob" },
                    { new Guid("55f05ad6-430f-4895-9017-8ae1a2fc3a65"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "With its intuitive interface, extensive service network, detailed record-keeping the car maintenance and service management website ensures a top-notch experience for all users.", false, null, 5, "Peter" },
                    { new Guid("b4777780-35ec-4e30-8dfc-d644e3938ba9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The car maintenance and service management website is user-friendly, offers a wide service network, effective communication, and robust record-keeping capabilities for a seamless experience.", false, null, 5, "David" },
                    { new Guid("c9b1486b-3e07-40cc-8134-98c8b2160424"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The website's interactive forum creates a dynamic space where car enthusiasts can connect, exchange valuable insights, and foster a supportive community for engaging discussions and information sharing.", false, null, 5, "Paul" },
                    { new Guid("cd4b6684-e07a-4391-ac4a-aafc2fabd30b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Experience exceptional car maintenance and service management with a user-friendly website interface, comprehensive service network, efficient communication, thorough record-keeping, and prompt notifications for a hassle-free experience.", false, null, 5, "Michael" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("32edbfae-5b94-42b3-b6a2-191d20fbc82c"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("55f05ad6-430f-4895-9017-8ae1a2fc3a65"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("b4777780-35ec-4e30-8dfc-d644e3938ba9"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("c9b1486b-3e07-40cc-8134-98c8b2160424"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("cd4b6684-e07a-4391-ac4a-aafc2fabd30b"));

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiration",
                table: "UserRefreshToken");

            migrationBuilder.InsertData(
                table: "CarouselAdModels",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "Description", "IsDeleted", "ModifiedOn", "StarsRating", "UserFirstName" },
                values: new object[,]
                {
                    { new Guid("152de202-f765-4113-b00c-aa2247a79e6d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Experience exceptional car maintenance and service management with a user-friendly website interface, comprehensive service network, efficient communication, thorough record-keeping, and prompt notifications for a hassle-free experience.", false, null, 5, "Michael" },
                    { new Guid("3ad4564c-ac02-42cc-bdf4-5675537ce55d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The forum within the website fosters a vibrant community, enabling car owners to communicate, share valuable information, and seek advice for a collaborative and informative experience.", false, null, 5, "Bob" },
                    { new Guid("4abb8409-e764-4233-ab9a-43d82877111b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The website's interactive forum creates a dynamic space where car enthusiasts can connect, exchange valuable insights, and foster a supportive community for engaging discussions and information sharing.", false, null, 5, "Paul" },
                    { new Guid("69015006-2fdd-47a4-ac3f-8cc30d79f212"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "With its intuitive interface, extensive service network, detailed record-keeping the car maintenance and service management website ensures a top-notch experience for all users.", false, null, 5, "Peter" },
                    { new Guid("af5ddff3-f702-4beb-a498-4f287d1114ae"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The car maintenance and service management website is user-friendly, offers a wide service network, effective communication, and robust record-keeping capabilities for a seamless experience.", false, null, 5, "David" }
                });
        }
    }
}
