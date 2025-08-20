using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrzewaAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemovedNotUsedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_TreeReports_TreeReportId",
                table: "Votes");

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
                name: "Municipalities");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "TreeReports");

            migrationBuilder.DropTable(
                name: "SpeciesImages");

            migrationBuilder.DropColumn(
                name: "SubmissionsCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VerificationsCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "TreeSpecies");

            migrationBuilder.DropColumn(
                name: "SeasonalChanges",
                table: "TreeSpecies");

            migrationBuilder.RenameColumn(
                name: "VoteType",
                table: "Votes",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "TreeReportId",
                table: "Votes",
                newName: "TreeSubmissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_UserId_TreeReportId",
                table: "Votes",
                newName: "IX_Votes_UserId_TreeSubmissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_TreeReportId",
                table: "Votes",
                newName: "IX_Votes_TreeSubmissionId");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Votes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TreeSpeciesImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TreeSpeciesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AltText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeSpeciesImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreeSpeciesImage_TreeSpecies_TreeSpeciesId",
                        column: x => x.TreeSpeciesId,
                        principalTable: "TreeSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreeSubmissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpeciesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Circumference = table.Column<int>(type: "int", maxLength: 6, nullable: false),
                    Height = table.Column<double>(type: "float", nullable: true),
                    Condition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAlive = table.Column<bool>(type: "bit", nullable: false),
                    EstimatedAge = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Videos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMonument = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreeSubmissions_TreeSpecies_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "TreeSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TreeSubmissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreeSpeciesImage_TreeSpeciesId",
                table: "TreeSpeciesImage",
                column: "TreeSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeSubmissions_SpeciesId",
                table: "TreeSubmissions",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeSubmissions_UserId",
                table: "TreeSubmissions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_TreeSubmissions_TreeSubmissionId",
                table: "Votes",
                column: "TreeSubmissionId",
                principalTable: "TreeSubmissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_TreeSubmissions_TreeSubmissionId",
                table: "Votes");

            migrationBuilder.DropTable(
                name: "TreeSpeciesImage");

            migrationBuilder.DropTable(
                name: "TreeSubmissions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Votes");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Votes",
                newName: "VoteType");

            migrationBuilder.RenameColumn(
                name: "TreeSubmissionId",
                table: "Votes",
                newName: "TreeReportId");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_UserId_TreeSubmissionId",
                table: "Votes",
                newName: "IX_Votes_UserId_TreeReportId");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_TreeSubmissionId",
                table: "Votes",
                newName: "IX_Votes_TreeReportId");

            migrationBuilder.AddColumn<int>(
                name: "SubmissionsCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VerificationsCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "TreeSpecies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SeasonalChanges",
                table: "TreeSpecies",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Municipalities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
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
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationGuide = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LatinName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PolishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeasonalChanges = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "SpeciesImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
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
                name: "TreeReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpeciesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Circumference = table.Column<int>(type: "int", maxLength: 6, nullable: false),
                    CommentsCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    EstimatedAge = table.Column<int>(type: "int", nullable: true),
                    FeaturedLegend = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsAlive = table.Column<bool>(type: "bit", nullable: false),
                    IsNatureMonument = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    LocationDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VotesCount = table.Column<int>(type: "int", nullable: false)
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
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MunicipalityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TreeReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttachmentsZipUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Justification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TreeReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_TreeReports_TreeReportId",
                table: "Votes",
                column: "TreeReportId",
                principalTable: "TreeReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
