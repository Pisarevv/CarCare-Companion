using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarCare_Companion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedTripRecordCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("0d452bde-7768-408e-9a15-d405575958ff"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("128587a8-9c82-460d-8fdf-83fe34f615c5"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("5a7db827-f793-48ef-b5cb-21a3d1f8551c"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("b1535c82-0010-4c37-9f5f-d0b90891cf53"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("ff977572-61ca-4923-9b48-31751c882f87"));

            migrationBuilder.AlterColumn<decimal>(
                name: "FuelPrice",
                table: "TripRecords",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                comment: "The price of the fuel during the trip in levs",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true,
                oldComment: "The price of the fuel during the trip");

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "TripRecords",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                comment: "The cost of the trip");

            migrationBuilder.InsertData(
                table: "CarouselAdModels",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "Description", "IsDeleted", "ModifiedOn", "StarsRating", "UserFirstName" },
                values: new object[,]
                {
                    { new Guid("0da9566f-6f1a-4ab0-9b70-3606070c0323"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The forum within the website fosters a vibrant community, enabling car owners to communicate, share valuable information, and seek advice for a collaborative and informative experience.", false, null, 5, "Bob" },
                    { new Guid("351d680c-603d-4e4f-9519-1767778ac0fa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The website's interactive forum creates a dynamic space where car enthusiasts can connect, exchange valuable insights, and foster a supportive community for engaging discussions and information sharing.", false, null, 5, "Paul" },
                    { new Guid("4785cfd5-e520-42f0-ba4c-a1e1382a8c51"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Experience exceptional car maintenance and service management with a user-friendly website interface, comprehensive service network, efficient communication, thorough record-keeping, and prompt notifications for a hassle-free experience.", false, null, 5, "Michael" },
                    { new Guid("77e19989-ea40-4540-8227-6464d6c3a9a8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "With its intuitive interface, extensive service network, detailed record-keeping the car maintenance and service management website ensures a top-notch experience for all users.", false, null, 5, "Peter" },
                    { new Guid("8f65b503-2bdd-4ba9-a28f-49f8dac7318b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The car maintenance and service management website is user-friendly, offers a wide service network, effective communication, and robust record-keeping capabilities for a seamless experience.", false, null, 5, "David" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("0da9566f-6f1a-4ab0-9b70-3606070c0323"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("351d680c-603d-4e4f-9519-1767778ac0fa"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("4785cfd5-e520-42f0-ba4c-a1e1382a8c51"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("77e19989-ea40-4540-8227-6464d6c3a9a8"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("8f65b503-2bdd-4ba9-a28f-49f8dac7318b"));

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "TripRecords");

            migrationBuilder.AlterColumn<decimal>(
                name: "FuelPrice",
                table: "TripRecords",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                comment: "The price of the fuel during the trip",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true,
                oldComment: "The price of the fuel during the trip in levs");

            migrationBuilder.InsertData(
                table: "CarouselAdModels",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "Description", "IsDeleted", "ModifiedOn", "StarsRating", "UserFirstName" },
                values: new object[,]
                {
                    { new Guid("0d452bde-7768-408e-9a15-d405575958ff"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "With its intuitive interface, extensive service network, detailed record-keeping the car maintenance and service management website ensures a top-notch experience for all users.", false, null, 5, "Peter" },
                    { new Guid("128587a8-9c82-460d-8fdf-83fe34f615c5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Experience exceptional car maintenance and service management with a user-friendly website interface, comprehensive service network, efficient communication, thorough record-keeping, and prompt notifications for a hassle-free experience.", false, null, 5, "Michael" },
                    { new Guid("5a7db827-f793-48ef-b5cb-21a3d1f8551c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The website's interactive forum creates a dynamic space where car enthusiasts can connect, exchange valuable insights, and foster a supportive community for engaging discussions and information sharing.", false, null, 5, "Paul" },
                    { new Guid("b1535c82-0010-4c37-9f5f-d0b90891cf53"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The forum within the website fosters a vibrant community, enabling car owners to communicate, share valuable information, and seek advice for a collaborative and informative experience.", false, null, 5, "Bob" },
                    { new Guid("ff977572-61ca-4923-9b48-31751c882f87"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The car maintenance and service management website is user-friendly, offers a wide service network, effective communication, and robust record-keeping capabilities for a seamless experience.", false, null, 5, "David" }
                });
        }
    }
}
