using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarCare_Companion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "User last name",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "User first name",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Date of user creation");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                comment: "Last date of user modification");

            migrationBuilder.CreateTable(
                name: "FuelTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Fuel id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Name of the fuel")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Type id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Name of the type")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identifier"),
                    Make = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Make name"),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Model name"),
                    Mileage = table.Column<double>(type: "float", nullable: false, comment: "Mileage of the vehicle"),
                    FuelTypeId = table.Column<int>(type: "int", nullable: false, comment: "Used fuel type identifier"),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Vehicle owner identifier"),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of creation"),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Last date of modification"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Deleted status"),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Date of deleting")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_FuelTypes_FuelTypeId",
                        column: x => x.FuelTypeId,
                        principalTable: "FuelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identifier"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The title of the record"),
                    PerformedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of the performed service"),
                    Mileage = table.Column<double>(type: "float", nullable: false, comment: "Vehicle mileage"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Description of the service"),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The vehicle identifier"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of creation"),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Last date of modification"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Deleted status"),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Date of deleting")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRecords_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identifier"),
                    StartDestination = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "The start destination of the trip"),
                    EndDestination = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "The end destination of the trip"),
                    MileageTravelled = table.Column<double>(type: "float", nullable: false, comment: "The travelled destination on the trip"),
                    UsedFuel = table.Column<double>(type: "float", nullable: true, comment: "The used destination on the trip"),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The vehicle identifier"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of creation"),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Last date of modification"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Deleted status"),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Date of deleting")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripRecords_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FuelTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Petrol" },
                    { 2, "Diesel" },
                    { 3, "Electric" },
                    { 4, "Hybrid" },
                    { 5, "Petrol/LPG" }
                });

            migrationBuilder.InsertData(
                table: "VehicleTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Sedan" },
                    { 2, "Hatchback" },
                    { 3, "SUV" },
                    { 4, "Crossover" },
                    { 5, "Coupe" },
                    { 6, "Convertible" },
                    { 7, "Minivan" },
                    { 8, "Pickup Truck" },
                    { 9, "Van" },
                    { 10, "Truck" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRecords_VehicleId",
                table: "ServiceRecords",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_TripRecords_VehicleId",
                table: "TripRecords",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_FuelTypeId",
                table: "Vehicles",
                column: "FuelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_OwnerId",
                table: "Vehicles",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleTypeId",
                table: "Vehicles",
                column: "VehicleTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceRecords");

            migrationBuilder.DropTable(
                name: "TripRecords");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "FuelTypes");

            migrationBuilder.DropTable(
                name: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "User last name");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "User first name");
        }
    }
}
