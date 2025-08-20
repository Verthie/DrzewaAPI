using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrzewaAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModelFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Videos",
                table: "TreeSubmissions");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "TreeSubmissions",
                newName: "Lng");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "TreeSubmissions",
                newName: "Lat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lng",
                table: "TreeSubmissions",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "Lat",
                table: "TreeSubmissions",
                newName: "Latitude");

            migrationBuilder.AddColumn<string>(
                name: "Videos",
                table: "TreeSubmissions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
