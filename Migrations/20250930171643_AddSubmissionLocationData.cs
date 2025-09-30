using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrzewaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionLocationData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location_Commune",
                table: "TreeSubmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location_County",
                table: "TreeSubmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location_District",
                table: "TreeSubmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location_Province",
                table: "TreeSubmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TreeSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000007"),
                columns: new[] { "Location_Address", "Location_Commune", "Location_County", "Location_District", "Location_PlotNumber", "Location_Province" },
                values: new object[] { "Nieznany", "string", "string", "string", "112/2", "string" });

            migrationBuilder.UpdateData(
                table: "TreeSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000008"),
                columns: new[] { "Location_Address", "Location_Commune", "Location_County", "Location_District", "Location_PlotNumber", "Location_Province" },
                values: new object[] { "Nieznany", "string", "string", "string", "11/1", "string" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location_Commune",
                table: "TreeSubmissions");

            migrationBuilder.DropColumn(
                name: "Location_County",
                table: "TreeSubmissions");

            migrationBuilder.DropColumn(
                name: "Location_District",
                table: "TreeSubmissions");

            migrationBuilder.DropColumn(
                name: "Location_Province",
                table: "TreeSubmissions");

            migrationBuilder.UpdateData(
                table: "TreeSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000007"),
                columns: new[] { "Location_Address", "Location_PlotNumber" },
                values: new object[] { "111/2", null });

            migrationBuilder.UpdateData(
                table: "TreeSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000008"),
                columns: new[] { "Location_Address", "Location_PlotNumber" },
                values: new object[] { "210/1", null });
        }
    }
}
