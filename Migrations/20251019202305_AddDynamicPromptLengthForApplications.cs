using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrzewaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDynamicPromptLengthForApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeminiResponseMaxLength",
                table: "ApplicationTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GeminiResponseMinLength",
                table: "ApplicationTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ApplicationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000005"),
                columns: new[] { "GeminiResponseMaxLength", "GeminiResponseMinLength" },
                values: new object[] { 1200, 900 });

            migrationBuilder.UpdateData(
                table: "ApplicationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000006"),
                columns: new[] { "GeminiResponseMaxLength", "GeminiResponseMinLength" },
                values: new object[] { 650, 400 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeminiResponseMaxLength",
                table: "ApplicationTemplates");

            migrationBuilder.DropColumn(
                name: "GeminiResponseMinLength",
                table: "ApplicationTemplates");
        }
    }
}
