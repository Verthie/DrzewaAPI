using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrzewaAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserAvatars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000001"),
                column: "Avatar",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000002"),
                column: "Avatar",
                value: null);
        }
    }
}
