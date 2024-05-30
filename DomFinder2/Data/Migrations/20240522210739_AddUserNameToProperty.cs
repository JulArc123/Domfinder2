using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomFinder2.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNameToProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Properties");
        }
    }
}
