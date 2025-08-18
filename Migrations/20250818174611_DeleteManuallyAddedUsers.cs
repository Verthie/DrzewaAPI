using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DrzewaAPI.Migrations
{
    /// <inheritdoc />
    public partial class DeleteManuallyAddedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111114"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "Email", "FirstName", "LastName", "PasswordHash", "Phone", "RegistrationDate", "SubmissionsCount", "VerificationsCount" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "https://images.pexels.com/photos/220453/pexels-photo-220453.jpeg?w=100&h=100&fit=crop", "adam.wolkin@email.com", "Adam", "Kowalski", "VerySafe", null, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 45 },
                    { new Guid("11111111-1111-1111-1111-111111111112"), "https://images.pexels.com/photos/415829/pexels-photo-415829.jpeg?w=100&h=100&fit=crop", "maria.kowalska@email.com", "Maria", "Nowak", "VerySafe", null, new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 32 },
                    { new Guid("11111111-1111-1111-1111-111111111113"), "https://images.pexels.com/photos/614810/pexels-photo-614810.jpeg?w=100&h=100&fit=crop", "piotr.nowak@email.com", "Piotr", "Wiśniewski", "VerySafe", null, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, 28 },
                    { new Guid("11111111-1111-1111-1111-111111111114"), "https://images.pexels.com/photos/733872/pexels-photo-733872.jpeg?w=100&h=100&fit=crop", "anna.wisniowska@email.com", "Anna", "Zielińska", "VerySafe", null, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 22, 67 }
                });
        }
    }
}
