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
                name: "Communes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreeSpecies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PolishName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LatinName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IdentificationGuide = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeasonalChanges_Spring = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeasonalChanges_Summer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeasonalChanges_Autumn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeasonalChanges_Winter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Traits_MaxHeight = table.Column<int>(type: "int", nullable: true),
                    Traits_Lifespan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Traits_NativeToPoland = table.Column<bool>(type: "bit", nullable: true)
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
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Role = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    EmailVerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommuneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HtmlTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fields = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationTemplates_Communes_CommuneId",
                        column: x => x.CommuneId,
                        principalTable: "Communes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TreeSpeciesImageDto",
                columns: table => new
                {
                    TreeSpeciesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AltText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeSpeciesImageDto", x => new { x.TreeSpeciesId, x.Id });
                    table.ForeignKey(
                        name: "FK_TreeSpeciesImageDto_TreeSpecies_TreeSpeciesId",
                        column: x => x.TreeSpeciesId,
                        principalTable: "TreeSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailVerificationTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TokenType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerificationTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailVerificationTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
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
                    Location_Lat = table.Column<double>(type: "float", nullable: false),
                    Location_Lng = table.Column<double>(type: "float", nullable: false),
                    Location_Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location_PlotNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Circumference = table.Column<int>(type: "int", maxLength: 6, nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Condition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAlive = table.Column<bool>(type: "bit", nullable: false),
                    EstimatedAge = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TreeSubmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeneratedHtmlContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneratedPdfPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommuneId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_ApplicationTemplates_ApplicationTemplateId",
                        column: x => x.ApplicationTemplateId,
                        principalTable: "ApplicationTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Communes_CommuneId",
                        column: x => x.CommuneId,
                        principalTable: "Communes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Applications_TreeSubmissions_TreeSubmissionId",
                        column: x => x.TreeSubmissionId,
                        principalTable: "TreeSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TreeSubmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    DatePosted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsLegend = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_TreeSubmissions_TreeSubmissionId",
                        column: x => x.TreeSubmissionId,
                        principalTable: "TreeSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TreeVotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TreeSubmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreeVotes_TreeSubmissions_TreeSubmissionId",
                        column: x => x.TreeSubmissionId,
                        principalTable: "TreeSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreeVotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommentVotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentVotes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentVotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Communes",
                columns: new[] { "Id", "Address", "City", "CreatedDate", "Email", "LastModifiedDate", "Name", "Phone", "PostalCode", "Province", "Website" },
                values: new object[,]
                {
                    { new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000003"), "Plac Bankowy 3/5", "Warszawa", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "sekretariat@um.warszawa.pl", null, "Gmina Warszawa", "+48 22 443 01 00", "00-950", "Mazowieckie", "https://www.warszawa.pl" },
                    { new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000004"), "Pl. Wszystkich Świętych 3-4", "Kraków", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ws.umk@um.krakow.pl", null, "Gmina Kraków", "+48 12 616 5555", "31-004", "Małopolskie", "https://www.krakow.pl" },
                    { new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000009"), "Pl. Nowy Targ 1-8", "Wrocław", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "urzad@um.wroc.pl", null, "Gmina Wrocław", "+48 71 777 7777", "50-141", "Dolnośląskie", "https://www.wroclaw.pl" },
                    { new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000010"), "Ul. Nowe Ogrody 8/12", "Gdańsk", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "prezydent@gedanopedia.pl", null, "Gmina Gdańsk", "+48 58 323 7000", "80-803", "Pomorskie", "https://www.gdansk.pl" },
                    { new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000011"), "Pl. Kolegiacki 17", "Poznań", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "um@poznan.pl", null, "Gmina Poznań", "+48 61 878 5000", "61-841", "Wielkopolskie", "https://www.poznan.pl" },
                    { new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000012"), "Ul. Piotrkowska 104", "Łódź", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "um@uml.lodz.pl", null, "Gmina Łódź", "+48 42 638 4000", "90-926", "Łódzkie", "https://www.lodz.pl" },
                    { new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000013"), "Ul. Młyńska 4", "Katowice", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "umkatowice@katowice.eu", null, "Gmina Katowice", "+48 32 259 3500", "40-098", "Śląskie", "https://www.katowice.eu" },
                    { new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000014"), "Ul. Lubelska 9", "Rzeszów", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "um@erzeszow.pl", null, "Gmina Rzeszów", "+48 17 875 4000", "35-021", "Podkarpackie", "https://www.rzeszow.pl" }
                });

            migrationBuilder.InsertData(
                table: "TreeSpecies",
                columns: new[] { "Id", "SeasonalChanges_Autumn", "SeasonalChanges_Spring", "SeasonalChanges_Summer", "SeasonalChanges_Winter", "Traits_Lifespan", "Traits_MaxHeight", "Traits_NativeToPoland", "Description", "Family", "IdentificationGuide", "LatinName", "PolishName" },
                values: new object[] { new Guid("c6d5f2b5-bc4a-4f3d-9b68-13e2a62f3ed8"), "Liście żółto-brązowe, opadają późno w sezonie. Dojrzałe żołędzie opadają i są zbierane przez zwierzęta", "Młode liście jasno-zielone, często z czerwonawym nalotem. Kwitnienie w maju - kotki męskie i niewielkie kwiaty żeńskie", "Liście ciemno-zielone, gęsta korona dająca dużo cienia. Rozwijają się żołędzie", "Charakterystyczna sylwetka z grubym pniem i rozłożystymi gałęziami. Kora wyraźnie bruzdowna", "Ponad 1000 lat", 40, true, "Dąb szypułkowy to jeden z najważniejszych gatunków drzew w Polsce. Może żyć ponad 1000 lat i osiągać wysokość do 40 metrów. Jest symbolem siły, trwałości i mądrości w kulturze słowiańskiej. Drewno dębu było używane do budowy statków, domów i mebli przez wieki.", "Fagaceae", "[\"Li\\u015Bcie z wyra\\u017Anymi wci\\u0119ciami, bez szypu\\u0142ek lub z bardzo kr\\u00F3tkimi szypu\\u0142kami\",\"\\u017Bo\\u0142\\u0119dzie na d\\u0142ugich szypu\\u0142kach (2-8 cm), dojrzewaj\\u0105 jesieni\\u0105\",\"Kora szara, g\\u0142\\u0119boko bruzdowna u starych okaz\\u00F3w, g\\u0142adka u m\\u0142odych\",\"Korona szeroka, roz\\u0142o\\u017Cysta, charakterystyczny pokr\\u00F3j \\u0022parasola\\u0022\",\"P\\u0105ki skupione na ko\\u0144cach p\\u0119d\\u00F3w, jajowate, br\\u0105zowe\"]", "Quercus Robur", "Dąb szypułkowy" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Avatar", "City", "Email", "EmailVerifiedAt", "FirstName", "IsEmailVerified", "LastName", "PasswordHash", "Phone", "PostalCode", "RegistrationDate", "Role" },
                values: new object[] { new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000001"), null, null, null, "mod@example.com", null, "Adam", false, "Kowalski", "AQAAAAIAAYagAAAAEHrSf4c5BhE6GMi8qlT3Q+oj6mJdQ2OAuPNUgxuc2sFGCxCeqhJwGOEUTqjSuPCFRw==", null, null, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Avatar", "City", "Email", "EmailVerifiedAt", "FirstName", "IsEmailVerified", "LastName", "PasswordHash", "Phone", "PostalCode", "RegistrationDate" },
                values: new object[] { new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000002"), null, null, null, "user@example.com", null, "Jan", false, "Kowalski", "AQAAAAIAAYagAAAAEDk+b31OOCvyrUQRFQztUECMUI+lPATVktwSn0Uysc66qax8wCdiejpv2Rd1YuophQ==", null, null, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "ApplicationTemplates",
                columns: new[] { "Id", "CommuneId", "CreatedDate", "Description", "Fields", "HtmlTemplate", "IsActive", "LastModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000005"), new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000003"), new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Standardowy szablon wniosku o rejestrację drzewa jako pomnika przyrody", "[{\"Name\":\"justification\",\"Label\":\"Uzasadnienie wniosku\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 opisa\\u0107 dlaczego drzewo powinno zosta\\u0107 obj\\u0119te ochron\\u0105...\",\"Options\":null,\"Validation\":{\"MinLength\":50,\"MaxLength\":1000,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Uzasadnienie musi mie\\u0107 od 50 do 1000 znak\\u00F3w\"},\"HelpText\":\"Opisz walory przyrodnicze, historyczne lub krajobrazowe drzewa\",\"Order\":1},{\"Name\":\"estimated_care_cost\",\"Label\":\"Szacowany koszt rocznej opieki (z\\u0142)\",\"Type\":1,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"np. 500\",\"Options\":null,\"Validation\":{\"MinLength\":null,\"MaxLength\":null,\"Pattern\":null,\"Min\":0,\"Max\":10000,\"ValidationMessage\":\"Koszt musi by\\u0107 liczb\\u0105 od 0 do 10000\"},\"HelpText\":\"Przewidywany koszt opieki nad drzewem w ci\\u0105gu roku\",\"Order\":2},{\"Name\":\"responsible_person\",\"Label\":\"Osoba odpowiedzialna za opiek\\u0119\",\"Type\":0,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Imi\\u0119 i nazwisko\",\"Options\":null,\"Validation\":{\"MinLength\":3,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":null},\"HelpText\":null,\"Order\":3},{\"Name\":\"contact_phone\",\"Label\":\"Telefon kontaktowy osoby odpowiedzialnej\",\"Type\":3,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"\\u002B48 123 456 789\",\"Options\":null,\"Validation\":{\"MinLength\":null,\"MaxLength\":null,\"Pattern\":\"^\\\\\\u002B?[0-9\\\\s\\\\-\\\\(\\\\)]{9,15}$\",\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Numer telefonu musi zawiera\\u0107 9-15 cyfr\"},\"HelpText\":null,\"Order\":4},{\"Name\":\"care_agreement\",\"Label\":\"Zobowi\\u0105zuj\\u0119 si\\u0119 do sprawowania opieki nad drzewem\",\"Type\":9,\"IsRequired\":true,\"DefaultValue\":\"false\",\"Placeholder\":null,\"Options\":null,\"Validation\":null,\"HelpText\":\"Wymagane potwierdzenie zobowi\\u0105zania\",\"Order\":5}]", "<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>Wniosek o rejestrację pomnika przyrody</title></head><body><h1>WNIOSEK O REJESTRACJĘ POMNIKA PRZYRODY</h1><div><h3>{{commune_name}}</h3><p>{{commune_address}}, {{commune_city}} {{commune_postal_code}}</p></div><div><h3>Dane wnioskodawcy:</h3><p>Imię i nazwisko: {{user_full_name}}</p><p>Adres: {{user_address}}, {{user_city}} {{user_postal_code}}</p><p>Telefon: {{user_phone}}</p><p>Email: {{user_email}}</p></div><div><h3>Dane drzewa:</h3><p>Gatunek: {{tree_species_polish}}</p><p>Obwód: {{tree_circumference}} cm</p><p>Wysokość: {{tree_height}} m</p><p>Wiek: {{tree_estimated_age}} lat</p><p>Stan: {{tree_condition}}</p></div><div><h3>Dodatkowe informacje:</h3><p>Uzasadnienie: {{justification}}</p><p>Przewidywany koszt opieki: {{estimated_care_cost}} zł/rok</p><p>Osoba odpowiedzialna: {{responsible_person}}</p><p>Telefon kontaktowy: {{contact_phone}}</p></div><div><p>Data: {{submission_date}}</p><p>Podpis: ................................</p></div></body></html>", true, null, "Wniosek o rejestrację pomnika przyrody" },
                    { new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000006"), new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000004"), new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Standardowy szablon wniosku o rejestrację drzewa jako pomnika przyrody", "[{\"Name\":\"plot\",\"Label\":\"Dzia\\u0142ka\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 dzia\\u0142k\\u0119 na kt\\u00F3rej znajduje si\\u0119 pomnik przyrody\",\"Options\":null,\"Validation\":{\"MinLength\":2,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 2 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":1},{\"Name\":\"cadastral_district\",\"Label\":\"Obr\\u0119b ewidencyjny\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 obr\\u0119b ewidencyjny\",\"Options\":null,\"Validation\":{\"MinLength\":2,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 2 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":2},{\"Name\":\"record_keeping_unit\",\"Label\":\"Jednostka ewidencyjna\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 jednostk\\u0119 ewidencyjn\\u0105\",\"Options\":null,\"Validation\":{\"MinLength\":2,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 2 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":3},{\"Name\":\"ownership_form\",\"Label\":\"Forma w\\u0142asno\\u015Bci\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 form\\u0119 w\\u0142asno\\u015Bci\",\"Options\":null,\"Validation\":{\"MinLength\":2,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 2 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":4},{\"Name\":\"land_type\",\"Label\":\"Rodzaj grunt\\u00F3w\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 rodzaj grunt\\u00F3w\",\"Options\":null,\"Validation\":{\"MinLength\":2,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 2 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":5},{\"Name\":\"study_name\",\"Label\":\"Nazwa opracowania\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 nazw\\u0119 opracowania\",\"Options\":null,\"Validation\":{\"MinLength\":2,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 2 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":6},{\"Name\":\"study_author\",\"Label\":\"Autor\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 imi\\u0119 i nazwisko autora opracowania\",\"Options\":null,\"Validation\":{\"MinLength\":2,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 2 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":7}]", "<!DOCTYPE html><html><meta charset=UTF-8><style>body{font-family:Arial,sans-serif;font-size:12px}.header{text-align:left;margin:0 5px 20px 5px;font-size:12px}.title{margin:42px 5px 42px 5px}.title h1{text-align:center;font-size:22px;font-weight:300;margin:0}.title h2{text-align:center;font-size:15px;margin:0 0 12px 0}.title p{font-size:14px}table{border-collapse:collapse;margin:auto}td{border:1px solid #000;padding:4px 8px 8px 8px;vertical-align:top}.number-col{width:30px;text-align:center}.question-col{width:42%}.answer-col{width:55%}.footer{display:flex;justify-content:space-between;align-items:flex-start;margin:64px 5px 0 5px}.footer *{margin:0}.signature{display:flex;flex-direction:column;text-align:center;justify-content:center}.signature-text{font-size:10px}</style><div class=header>Załącznik do procedury WS-13</div><div class=title><h1>Wniosek</h1><h2>o uznanie obiektu przyrodniczego za pomnik przyrody</h2><p>na podstawie Art. 6 ust. 1 pkt 6, art. 40, art. 44 ustawy z dnia 16 kwietnia 2004 r. o ochronie przyrody.</div><table><tr><td class=number-col>1.<td class=question-col>Imię i nazwisko wnioskodawcy / nazwa wnioskodawcy<br>Adres / siedziba wnioskodawcy<td class=answer-col>{{user_full_name}}<br>{{user_address}}<br>{{user_city}}, {{user_postal_code}}<tr><td class=number-col>2.<td class=question-col>Nazwa i rodzaj pomnika przyrody<td class=answer-col>Nazwa polska: {{tree_species_polish}}<br>Nazwa łacińska: {{tree_species_latin}}<br>Rodzaj: drzewo<tr><td class=number-col>3.<td class=question-col>Określenie położenia geograficznego i administracyjnego pomnika przyrody (działka, obręb ewidencyjny, jednostka ewidencyjna)<td class=answer-col>Położenie geograficzne: {{geographic_location_lat}} lat, {{geographic_location_long}} long<br>Działka: {{plot}}<br>Obręb ewidencyjny: {{cadastral_district}}<br>Jednostka ewidencyjna: {{record_keeping_unit}}<tr><td class=number-col>4.<td class=question-col>Wskazanie formy własności i rodzajów gruntów<td class=answer-col>Forma własności: {{ownership_form}}<br>Rodzaj gruntów: {{land_type}}<tr><td class=number-col>5.<td class=question-col>Wskazanie mapy obrazującej lokalizację pomnika przyrody<td class=answer-col><tr><td class=number-col>6.<td class=question-col>Krótki opis pomnika przyrody<br>- dla pomników przyrody żywej gatunek, wiek, pierśnica, wysokość, rozpiętość korony, stan zdrowotny,<br>- dla pomników przyrody nieżywej typ, rodzaj, wielkość źródła, wodospadu, głazu, jaskini itp.<td class=answer-col>Wiek: {{tree_estimated_age}}<br>Pierśnica: {{tree_circumference}} cm<br>Wysokość: {{tree_height}} m<br>Rozpiętość:<br>Stan zdrowotny: {{tree_condition}}<tr><td class=number-col>7.<td class=question-col>Nazwa, autor opracowania potwierdzającego wartości przyrodnicze obiektu<td class=answer-col>Nazwa opracowania: {{study_name}}<br>Autor: {{study_author}}</table><div class=footer><p>{{commune_city}}, dn. {{generation_date}}<div class=signature><p>..............................................<p class=signature-text><em>(podpis)</em></div></div></body></html>", true, null, "Uznanie obiektu przyrodniczego za pomnik przyrody WS-13" }
                });

            migrationBuilder.InsertData(
                table: "TreeSubmissions",
                columns: new[] { "Id", "Location_Address", "Location_Lat", "Location_Lng", "Location_PlotNumber", "ApprovalDate", "Circumference", "Condition", "Description", "EstimatedAge", "Height", "Images", "IsAlive", "IsMonument", "SpeciesId", "Status", "SubmissionDate", "UserId" },
                values: new object[,]
                {
                    { new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000007"), "111/2", 52.526961399999998, 17.128484199999999, null, new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, "Dobra", "Pomnik przyrody", 100, 20.0, null, true, true, new Guid("c6d5f2b5-bc4a-4f3d-9b68-13e2a62f3ed8"), 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000002") },
                    { new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000008"), "210/1", 51.536917899999999, 17.866406699999999, null, null, 115, "Zła", "Dąb Pomnik przyrody", 350, 27.0, null, true, true, new Guid("c6d5f2b5-bc4a-4f3d-9b68-13e2a62f3ed8"), 0, new DateTime(2024, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000002") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicationTemplateId",
                table: "Applications",
                column: "ApplicationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CommuneId",
                table: "Applications",
                column: "CommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_TreeSubmissionId",
                table: "Applications",
                column: "TreeSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_UserId",
                table: "Applications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTemplates_CommuneId_Name",
                table: "ApplicationTemplates",
                columns: new[] { "CommuneId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TreeSubmissionId",
                table: "Comments",
                column: "TreeSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVotes_CommentId",
                table: "CommentVotes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVotes_UserId_CommentId",
                table: "CommentVotes",
                columns: new[] { "UserId", "CommentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Communes_Name",
                table: "Communes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerificationTokens_Token",
                table: "EmailVerificationTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerificationTokens_UserId",
                table: "EmailVerificationTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeSubmissions_SpeciesId",
                table: "TreeSubmissions",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeSubmissions_UserId",
                table: "TreeSubmissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeVotes_TreeSubmissionId",
                table: "TreeVotes",
                column: "TreeSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeVotes_UserId_TreeSubmissionId",
                table: "TreeVotes",
                columns: new[] { "UserId", "TreeSubmissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "CommentVotes");

            migrationBuilder.DropTable(
                name: "EmailVerificationTokens");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "TreeSpeciesImageDto");

            migrationBuilder.DropTable(
                name: "TreeVotes");

            migrationBuilder.DropTable(
                name: "ApplicationTemplates");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Communes");

            migrationBuilder.DropTable(
                name: "TreeSubmissions");

            migrationBuilder.DropTable(
                name: "TreeSpecies");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
