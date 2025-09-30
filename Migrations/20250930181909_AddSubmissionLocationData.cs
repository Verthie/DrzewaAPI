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
            migrationBuilder.AlterColumn<double>(
                name: "Circumference",
                table: "TreeSubmissions",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 6);

            migrationBuilder.AddColumn<double>(
                name: "CrownSpread",
                table: "TreeSubmissions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

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
                columns: new[] { "Location_Address", "Location_Commune", "Location_County", "Location_District", "Location_PlotNumber", "Location_Province", "Circumference", "CrownSpread" },
                values: new object[] { "Nieznany", "string", "string", "string", "112/2", "string", 100.0, 150.0 });

            migrationBuilder.UpdateData(
                table: "TreeSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000008"),
                columns: new[] { "Location_Address", "Location_Commune", "Location_County", "Location_District", "Location_PlotNumber", "Location_Province", "Circumference", "CrownSpread" },
                values: new object[] { "Nieznany", "string", "string", "string", "11/1", "string", 115.0, 150.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CrownSpread",
                table: "TreeSubmissions");

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

            migrationBuilder.AlterColumn<int>(
                name: "Circumference",
                table: "TreeSubmissions",
                type: "int",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "TreeSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000007"),
                columns: new[] { "Circumference", "Location_Address", "Location_PlotNumber" },
                values: new object[] { 100, "111/2", null });

            migrationBuilder.UpdateData(
                table: "TreeSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000008"),
                columns: new[] { "Circumference", "Location_Address", "Location_PlotNumber" },
                values: new object[] { 115, "210/1", null });
        }
    }
}
