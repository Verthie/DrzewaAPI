using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DrzewaAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Municipalities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreeSpecies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PolishName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LatinName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IdentificationGuide = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    SeasonalChanges = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeSpecies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreeSpeciesImages",
                columns: table => new
                {
                    TreeSpeciesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeSpeciesImages", x => new { x.TreeSpeciesId, x.ImageId });
                    table.ForeignKey(
                        name: "FK_TreeSpeciesImages_SpeciesImages_ImageId",
                        column: x => x.ImageId,
                        principalTable: "SpeciesImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreeSpeciesImages_TreeSpecies_TreeSpeciesId",
                        column: x => x.TreeSpeciesId,
                        principalTable: "TreeSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesAdditionRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PolishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LatinName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationGuide = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeasonalChanges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesAdditionRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpeciesAdditionRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreeReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpeciesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    LocationDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Circumference = table.Column<int>(type: "int", maxLength: 6, nullable: false),
                    IsAlive = table.Column<bool>(type: "bit", nullable: false),
                    EstimatedAge = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FeaturedLegend = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsNatureMonument = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VotesCount = table.Column<int>(type: "int", nullable: false),
                    CommentsCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreeReports_TreeSpecies_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "TreeSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TreeReports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MunicipalityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TreeReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Justification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PdfUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentsZipUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applications_TreeReports_TreeReportId",
                        column: x => x.TreeReportId,
                        principalTable: "TreeReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TreeReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsLegend = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_TreeReports_TreeReportId",
                        column: x => x.TreeReportId,
                        principalTable: "TreeReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreeConditionTags",
                columns: table => new
                {
                    TreeReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeConditionTags", x => new { x.TagId, x.TreeReportId });
                    table.ForeignKey(
                        name: "FK_TreeConditionTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreeConditionTags_TreeReports_TreeReportId",
                        column: x => x.TreeReportId,
                        principalTable: "TreeReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreeReportAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TreeReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeReportAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreeReportAttachments_TreeReports_TreeReportId",
                        column: x => x.TreeReportId,
                        principalTable: "TreeReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TreeReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votes_TreeReports_TreeReportId",
                        column: x => x.TreeReportId,
                        principalTable: "TreeReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Municipalities",
                columns: new[] { "Id", "Address", "City", "Email", "Name", "Province", "ZipCode" },
                values: new object[] { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Street 15", "Warszawa", "urząd@warszawa.pl", "GminaWarszawska", "Mazowieckie", "24-040" });

            migrationBuilder.InsertData(
                table: "SpeciesImages",
                columns: new[] { "Id", "Description", "ImageUrl", "Type" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Zdjęcie dęba szypułkowego", "fileUrl", 0 },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "Zdjęcie liścia sosny zwyczajnej", "fileUrl", 2 }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Posusz" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Niestabilny" }
                });

            migrationBuilder.InsertData(
                table: "TreeSpecies",
                columns: new[] { "Id", "Category", "Description", "IdentificationGuide", "LatinName", "PolishName", "SeasonalChanges" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222222"), 1, "Dąb szypułkowy to jeden z najpopularniejszych gatunków drzew w Polsce.", null, "Quercus robur", "Dąb szypułkowy", null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 0, "Sosna zwyczajna to charakterystyczne drzewo iglaste występujące w Polsce.", null, "Pinus sylvestris", "Sosna zwyczajna", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "IsActive", "LastLoginAt", "LastName", "PasswordHash", "Phone" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ekowojownik@gmail.com", "Eko", false, null, "Wojownik", "VerySafe", null });

            migrationBuilder.InsertData(
                table: "TreeReports",
                columns: new[] { "Id", "Circumference", "CommentsCount", "CreatedAt", "Description", "EstimatedAge", "FeaturedLegend", "IsAlive", "IsNatureMonument", "IsVerified", "Latitude", "LocationDescription", "Longitude", "SpeciesId", "Status", "UserId", "VotesCount" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), 300, 0, new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fajne drzewo", 150, null, true, true, true, 500.0, "Miasto: Warszawa\nUlica: 3 maja", 100.0, new Guid("22222222-2222-2222-2222-222222222222"), 1, new Guid("11111111-1111-1111-1111-111111111111"), 0 },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 220, 0, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Martwe drzewo", 240, null, false, false, false, 400.0, "Miasto: Warszawa\nUlica: Wyspiańskiego", 210.0, new Guid("33333333-3333-3333-3333-333333333333"), 0, new Guid("11111111-1111-1111-1111-111111111111"), 0 }
                });

            migrationBuilder.InsertData(
                table: "TreeSpeciesImages",
                columns: new[] { "ImageId", "TreeSpeciesId" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("33333333-3333-3333-3333-333333333333") }
                });

            migrationBuilder.InsertData(
                table: "TreeConditionTags",
                columns: new[] { "TagId", "TreeReportId" },
                values: new object[,]
                {
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("44444444-4444-4444-4444-444444444444") }
                });

            migrationBuilder.InsertData(
                table: "TreeReportAttachments",
                columns: new[] { "Id", "FileName", "FileSize", "FileUrl", "TreeReportId", "Type", "UploadedAt" },
                values: new object[,]
                {
                    { new Guid("88888888-8888-8888-8888-888888888888"), "Obrazek pnia dębu", 0L, "fileUrl", new Guid("44444444-4444-4444-4444-444444444444"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "Obrazek liścia dębu", 0L, "fileUrl", new Guid("44444444-4444-4444-4444-444444444444"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Obrazek pnia sosny", 0L, "fileUrl", new Guid("55555555-5555-5555-5555-555555555555"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_MunicipalityId",
                table: "Applications",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_TreeReportId",
                table: "Applications",
                column: "TreeReportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_UserId",
                table: "Applications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TreeReportId",
                table: "Comments",
                column: "TreeReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesAdditionRequests_UserId",
                table: "SpeciesAdditionRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeConditionTags_TreeReportId",
                table: "TreeConditionTags",
                column: "TreeReportId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeReportAttachments_TreeReportId",
                table: "TreeReportAttachments",
                column: "TreeReportId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeReports_SpeciesId",
                table: "TreeReports",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeReports_UserId",
                table: "TreeReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeSpeciesImages_ImageId",
                table: "TreeSpeciesImages",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_TreeReportId",
                table: "Votes",
                column: "TreeReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId_TreeReportId",
                table: "Votes",
                columns: new[] { "UserId", "TreeReportId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "SpeciesAdditionRequests");

            migrationBuilder.DropTable(
                name: "TreeConditionTags");

            migrationBuilder.DropTable(
                name: "TreeReportAttachments");

            migrationBuilder.DropTable(
                name: "TreeSpeciesImages");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "Municipalities");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "SpeciesImages");

            migrationBuilder.DropTable(
                name: "TreeReports");

            migrationBuilder.DropTable(
                name: "TreeSpecies");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
