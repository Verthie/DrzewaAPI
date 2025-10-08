using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrzewaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTreeTagParameters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condition",
                table: "TreeSubmissions");

            migrationBuilder.AddColumn<string>(
                name: "Environment",
                table: "TreeSubmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Health",
                table: "TreeSubmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Soil",
                table: "TreeSubmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TreeSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000007"),
                columns: new[] { "Environment", "Health", "Soil" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "TreeSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000008"),
                columns: new[] { "Environment", "Health", "Soil" },
                values: new object[] { null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Environment",
                table: "TreeSubmissions");

            migrationBuilder.DropColumn(
                name: "Health",
                table: "TreeSubmissions");

            migrationBuilder.DropColumn(
                name: "Soil",
                table: "TreeSubmissions");

            migrationBuilder.AddColumn<string>(
                name: "Condition",
                table: "TreeSubmissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "TreeSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000007"),
                column: "Condition",
                value: "Dobra");

            migrationBuilder.UpdateData(
                table: "TreeSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000008"),
                column: "Condition",
                value: "Zła");
        }
    }
}
