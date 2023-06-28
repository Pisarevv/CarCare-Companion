using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarCare_Companion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCarouselAdModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarouselAdModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identifier"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StarsRating = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of creation"),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Last date of modification"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Deleted status"),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Date of deleting")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarouselAdModels", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CarouselAdModels",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "Description", "FirstName", "IsDeleted", "ModifiedOn", "StarsRating" },
                values: new object[,]
                {
                    { new Guid("27f62e44-700b-4be6-9da4-4b59b4627a23"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The website's interactive forum creates a dynamic space where car enthusiasts can connect, exchange valuable insights, and foster a supportive community for engaging discussions and information sharing.", "Paul", false, null, 5 },
                    { new Guid("4a874ecf-305e-4189-9e3c-4ed73213e30c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The car maintenance and service management website is user-friendly, offers a wide service network, effective communication, and robust record-keeping capabilities for a seamless experience.", "David", false, null, 5 },
                    { new Guid("6c8b274a-4a9f-49b0-b6a2-4ba672aa18dd"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Experience exceptional car maintenance and service management with a user-friendly website interface, comprehensive service network, efficient communication, thorough record-keeping, and prompt notifications for a hassle-free experience.", "Michael", false, null, 5 },
                    { new Guid("97175dbe-5652-49a5-aa63-4734a6530ec3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "With its intuitive interface, extensive service network, detailed record-keeping the car maintenance and service management website ensures a top-notch experience for all users.", "Peter", false, null, 5 },
                    { new Guid("c62c157f-2e8c-4086-84a3-df07e74ad867"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "The forum within the website fosters a vibrant community, enabling car owners to communicate, share valuable information, and seek advice for a collaborative and informative experience.", "Bob", false, null, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarouselAdModels");
        }
    }
}
