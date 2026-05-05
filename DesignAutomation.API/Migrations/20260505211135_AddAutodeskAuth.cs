using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignAutomation.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAutodeskAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthProvider",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AutodeskUserId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthProvider",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AutodeskUserId",
                table: "AspNetUsers");
        }
    }
}
