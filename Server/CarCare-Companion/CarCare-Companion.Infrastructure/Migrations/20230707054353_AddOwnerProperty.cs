using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarCare_Companion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("88720bc3-5e3c-4f8e-ad35-66494000843e"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("8d2dbbbf-28b2-48cd-9cbd-6932d5af8cde"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("9824c709-5b16-418f-a945-29ba0ff302ab"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("b046e559-3b67-4435-9569-14f4c3cc2dc0"));

            migrationBuilder.DeleteData(
                table: "CarouselAdModels",
                keyColumn: "Id",
                keyValue: new Guid("f90cf89f-5a7e-48c9-8b2e-cc819f74abd4"));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "TripRecords",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Vehicle owner identifier");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "ServiceRecords",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Vehicle owner identifier");

            migrationBuilder.CreateTable(
                name: "TaxRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identifier"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The title of the record"),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of tax validity"),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of tax validity end"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Description of the tax"),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Cost of the tax"),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The vehicle identifier"),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Vehicle owner identifier"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of creation"),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Last date of modification"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Deleted status"),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Date of deleting")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxRecord_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaxRecord_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_TripRecords_OwnerId",
                table: "TripRecords",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRecords_OwnerId",
                table: "ServiceRecords",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRecord_OwnerId",
                table: "TaxRecord",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRecord_VehicleId",
                table: "TaxRecord",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRecords_AspNetUsers_OwnerId",
                table: "ServiceRecords",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TripRecords_AspNetUsers_OwnerId",
                table: "TripRecords",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRecords_AspNetUsers_OwnerId",
                table: "ServiceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_TripRecords_AspNetUsers_OwnerId",
                table: "TripRecords");

            migrationBuilder.DropTable(
                name: "TaxRecord");

            migrationBuilder.DropIndex(
                name: "IX_TripRecords_OwnerId",
                table: "TripRecords");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRecords_OwnerId",
                table: "ServiceRecords");

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

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "TripRecords");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ServiceRecords");

            migrationBuilder.InsertData(
                table: "CarouselAdModels",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "Description", "IsDeleted", "ModifiedOn", "StarsRating", "UserFirstName" },
                values: new object[,]
                {
                    { new Guid("88720bc3-5e3c-4f8e-ad35-66494000843e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The forum within the website fosters a vibrant community, enabling car owners to communicate, share valuable information, and seek advice for a collaborative and informative experience.", false, null, 5, "Bob" },
                    { new Guid("8d2dbbbf-28b2-48cd-9cbd-6932d5af8cde"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The website's interactive forum creates a dynamic space where car enthusiasts can connect, exchange valuable insights, and foster a supportive community for engaging discussions and information sharing.", false, null, 5, "Paul" },
                    { new Guid("9824c709-5b16-418f-a945-29ba0ff302ab"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "With its intuitive interface, extensive service network, detailed record-keeping the car maintenance and service management website ensures a top-notch experience for all users.", false, null, 5, "Peter" },
                    { new Guid("b046e559-3b67-4435-9569-14f4c3cc2dc0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The car maintenance and service management website is user-friendly, offers a wide service network, effective communication, and robust record-keeping capabilities for a seamless experience.", false, null, 5, "David" },
                    { new Guid("f90cf89f-5a7e-48c9-8b2e-cc819f74abd4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Experience exceptional car maintenance and service management with a user-friendly website interface, comprehensive service network, efficient communication, thorough record-keeping, and prompt notifications for a hassle-free experience.", false, null, 5, "Michael" }
                });
        }
    }
}
