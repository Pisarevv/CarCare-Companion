using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarCare_Companion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VehicleModificatonWithAddedType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                table: "Vehicles");

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("6f993a08-f310-4050-bd78-71be9e2c34e7"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("7ae08e8b-fcf7-4983-9cc5-afb8d3bfda76"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("a8e74432-b2f3-4a8b-ad10-dcc53069aa97"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("b81928d5-27b6-4566-b3c2-3f58319a5bb2"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("ff0f8347-dcf3-44a5-af23-cdbab6e4161a"));

            migrationBuilder.AlterColumn<int>(
                name: "VehicleTypeId",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Vehicle type identifier",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "CarouselAdModels",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "Description", "IsDeleted", "ModifiedOn", "StarsRating", "UserFirstName" },
                values: new object[,]
                {
                    { new Guid("0422250b-1e83-458d-8ab0-c2faec67d6ec"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The forum within the website fosters a vibrant community, enabling car owners to communicate, share valuable information, and seek advice for a collaborative and informative experience.", false, null, 5, "Bob" },
                    { new Guid("43984d5e-4599-4690-b0a4-f965efe2f2ed"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Experience exceptional car maintenance and service management with a user-friendly website interface, comprehensive service network, efficient communication, thorough record-keeping, and prompt notifications for a hassle-free experience.", false, null, 5, "Michael" },
                    { new Guid("4f9622df-30be-4214-80be-9c79d782000c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The website's interactive forum creates a dynamic space where car enthusiasts can connect, exchange valuable insights, and foster a supportive community for engaging discussions and information sharing.", false, null, 5, "Paul" },
                    { new Guid("7a6e3094-0743-47b3-8b57-9516957d55c8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "With its intuitive interface, extensive service network, detailed record-keeping the car maintenance and service management website ensures a top-notch experience for all users.", false, null, 5, "Peter" },
                    { new Guid("99f1257c-18ae-4354-8365-2d51ebe071ce"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The car maintenance and service management website is user-friendly, offers a wide service network, effective communication, and robust record-keeping capabilities for a seamless experience.", false, null, 5, "David" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                table: "Vehicles",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                table: "Vehicles");

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("0422250b-1e83-458d-8ab0-c2faec67d6ec"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("43984d5e-4599-4690-b0a4-f965efe2f2ed"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("4f9622df-30be-4214-80be-9c79d782000c"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("7a6e3094-0743-47b3-8b57-9516957d55c8"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("99f1257c-18ae-4354-8365-2d51ebe071ce"));

            migrationBuilder.AlterColumn<int>(
                name: "VehicleTypeId",
                table: "Vehicles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Vehicle type identifier");

            migrationBuilder.InsertData(
                table: "CarouselAdModels",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "Description", "IsDeleted", "ModifiedOn", "StarsRating", "UserFirstName" },
                values: new object[,]
                {
                    { new Guid("6f993a08-f310-4050-bd78-71be9e2c34e7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The forum within the website fosters a vibrant community, enabling car owners to communicate, share valuable information, and seek advice for a collaborative and informative experience.", false, null, 5, "Bob" },
                    { new Guid("7ae08e8b-fcf7-4983-9cc5-afb8d3bfda76"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The car maintenance and service management website is user-friendly, offers a wide service network, effective communication, and robust record-keeping capabilities for a seamless experience.", false, null, 5, "David" },
                    { new Guid("a8e74432-b2f3-4a8b-ad10-dcc53069aa97"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The website's interactive forum creates a dynamic space where car enthusiasts can connect, exchange valuable insights, and foster a supportive community for engaging discussions and information sharing.", false, null, 5, "Paul" },
                    { new Guid("b81928d5-27b6-4566-b3c2-3f58319a5bb2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "With its intuitive interface, extensive service network, detailed record-keeping the car maintenance and service management website ensures a top-notch experience for all users.", false, null, 5, "Peter" },
                    { new Guid("ff0f8347-dcf3-44a5-af23-cdbab6e4161a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Experience exceptional car maintenance and service management with a user-friendly website interface, comprehensive service network, efficient communication, thorough record-keeping, and prompt notifications for a hassle-free experience.", false, null, 5, "Michael" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                table: "Vehicles",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id");
        }
    }
}
