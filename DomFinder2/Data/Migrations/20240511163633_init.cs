using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomFinder2.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    PropertyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Area = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rooms = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuildingType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: true),
                    TotalFloors = table.Column<int>(type: "int", nullable: true),
                    YearBuilt = table.Column<int>(type: "int", nullable: true),
                    OwnershipType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Rent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdvertiserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnergyCertificate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Internet = table.Column<bool>(type: "bit", nullable: false),
                    CableTV = table.Column<bool>(type: "bit", nullable: false),
                    Phone = table.Column<bool>(type: "bit", nullable: false),
                    AntiBurglaryBlinds = table.Column<bool>(type: "bit", nullable: false),
                    Monitoring = table.Column<bool>(type: "bit", nullable: false),
                    AlarmSystem = table.Column<bool>(type: "bit", nullable: false),
                    Intercom = table.Column<bool>(type: "bit", nullable: false),
                    ClosedArea = table.Column<bool>(type: "bit", nullable: false),
                    Furnished = table.Column<bool>(type: "bit", nullable: false),
                    WashingMachine = table.Column<bool>(type: "bit", nullable: false),
                    Fridge = table.Column<bool>(type: "bit", nullable: false),
                    Stove = table.Column<bool>(type: "bit", nullable: false),
                    Television = table.Column<bool>(type: "bit", nullable: false),
                    Dishwasher = table.Column<bool>(type: "bit", nullable: false),
                    Oven = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.PropertyID);
                    table.ForeignKey(
                        name: "FK_Properties_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_UserId1",
                table: "Properties",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Properties");
        }
    }
}
