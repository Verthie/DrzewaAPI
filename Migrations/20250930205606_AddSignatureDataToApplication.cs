using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrzewaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSignatureDataToApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Location_Address",
                table: "TreeSubmissions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<float>(
                name: "Signature_Height",
                table: "ApplicationTemplates",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Signature_Width",
                table: "ApplicationTemplates",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Signature_X",
                table: "ApplicationTemplates",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Signature_Y",
                table: "ApplicationTemplates",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.UpdateData(
                table: "ApplicationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000005"),
                columns: new[] { "Signature_Height", "Signature_Width", "Signature_X", "Signature_Y" },
                values: new object[] { 10f, 10f, 10f, 10f });

            migrationBuilder.UpdateData(
                table: "ApplicationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000006"),
                columns: new[] { "Signature_Height", "Signature_Width", "Signature_X", "Signature_Y" },
                values: new object[] { 10f, 10f, 10f, 10f });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Signature_Height",
                table: "ApplicationTemplates");

            migrationBuilder.DropColumn(
                name: "Signature_Width",
                table: "ApplicationTemplates");

            migrationBuilder.DropColumn(
                name: "Signature_X",
                table: "ApplicationTemplates");

            migrationBuilder.DropColumn(
                name: "Signature_Y",
                table: "ApplicationTemplates");

            migrationBuilder.AlterColumn<string>(
                name: "Location_Address",
                table: "TreeSubmissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
