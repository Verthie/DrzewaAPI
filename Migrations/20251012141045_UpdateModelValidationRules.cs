using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrzewaAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelValidationRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Avatar",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "TreeVotes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionDate",
                table: "TreeSubmissions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Legend",
                table: "TreeSubmissions",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TreeSubmissions",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TreeSpecies",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "EmailVerificationTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Communes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Communes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Communes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Communes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Communes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Communes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ApplicationTemplates",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Applications",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "ApplicationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000005"),
                columns: new[] { "Signature_Height", "Signature_Width", "Signature_Y", "Fields" },
                values: new object[] { 120f, 80f, 150f, "[{\"Name\":\"justification\",\"Label\":\"Uzasadnienie wniosku\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 opisa\\u0107 dlaczego drzewo powinno zosta\\u0107 obj\\u0119te ochron\\u0105...\",\"Options\":null,\"Validation\":{\"MinLength\":50,\"MaxLength\":1500,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Uzasadnienie musi mie\\u0107 od 50 do 1500 znak\\u00F3w\"},\"HelpText\":\"Opisz walory przyrodnicze, historyczne lub krajobrazowe drzewa\",\"Order\":1},{\"Name\":\"estimated_care_cost\",\"Label\":\"Szacowany koszt rocznej opieki (z\\u0142)\",\"Type\":1,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"np. 500\",\"Options\":null,\"Validation\":{\"MinLength\":null,\"MaxLength\":null,\"Pattern\":null,\"Min\":0,\"Max\":10000,\"ValidationMessage\":\"Koszt musi by\\u0107 liczb\\u0105 od 0 do 10000\"},\"HelpText\":\"Przewidywany koszt opieki nad drzewem w ci\\u0105gu roku\",\"Order\":2},{\"Name\":\"responsible_person\",\"Label\":\"Osoba odpowiedzialna za opiek\\u0119\",\"Type\":0,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Imi\\u0119 i nazwisko\",\"Options\":null,\"Validation\":{\"MinLength\":1,\"MaxLength\":200,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Imi\\u0119 i nazwisko powinno zawiera\\u0107 od 1 do 200 znak\\u00F3w\"},\"HelpText\":null,\"Order\":3},{\"Name\":\"contact_phone\",\"Label\":\"Telefon kontaktowy osoby odpowiedzialnej\",\"Type\":3,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"\\u002B48 123 456 789\",\"Options\":null,\"Validation\":{\"MinLength\":9,\"MaxLength\":20,\"Pattern\":\"^\\\\\\u002B?[0-9\\\\s\\\\-\\\\(\\\\)]{9,15}$\",\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Numer telefonu musi zawiera\\u0107 9-15 cyfr\"},\"HelpText\":null,\"Order\":4},{\"Name\":\"care_agreement\",\"Label\":\"Zobowi\\u0105zuj\\u0119 si\\u0119 do sprawowania opieki nad drzewem\",\"Type\":9,\"IsRequired\":true,\"DefaultValue\":\"false\",\"Placeholder\":null,\"Options\":null,\"Validation\":null,\"HelpText\":\"Wymagane potwierdzenie zobowi\\u0105zania\",\"Order\":5}]" });

            migrationBuilder.UpdateData(
                table: "ApplicationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000006"),
                columns: new[] { "Signature_Height", "Signature_Width", "Signature_X", "Signature_Y", "Fields" },
                values: new object[] { 120f, 80f, 410f, 150f, "[{\"Name\":\"justification\",\"Label\":\"Uzasadnienie wniosku\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 opisa\\u0107 dlaczego drzewo powinno zosta\\u0107 obj\\u0119te ochron\\u0105...\",\"Options\":null,\"Validation\":{\"MinLength\":50,\"MaxLength\":1500,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Uzasadnienie musi mie\\u0107 od 50 do 1500 znak\\u00F3w\"},\"HelpText\":\"Opisz walory przyrodnicze, historyczne lub krajobrazowe drzewa\",\"Order\":1},{\"Name\":\"location_plot\",\"Label\":\"Dzia\\u0142ka\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 dzia\\u0142k\\u0119 na kt\\u00F3rej znajduje si\\u0119 pomnik przyrody\",\"Options\":null,\"Validation\":{\"MinLength\":1,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 1 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":2},{\"Name\":\"location_district\",\"Label\":\"Obr\\u0119b ewidencyjny\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 obr\\u0119b ewidencyjny\",\"Options\":null,\"Validation\":{\"MinLength\":1,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 1 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":3},{\"Name\":\"record_keeping_unit\",\"Label\":\"Jednostka ewidencyjna\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 jednostk\\u0119 ewidencyjn\\u0105\",\"Options\":null,\"Validation\":{\"MinLength\":1,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 1 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":4},{\"Name\":\"ownership_form\",\"Label\":\"Forma w\\u0142asno\\u015Bci\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 form\\u0119 w\\u0142asno\\u015Bci\",\"Options\":null,\"Validation\":{\"MinLength\":1,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 1 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":5},{\"Name\":\"land_type\",\"Label\":\"Rodzaj grunt\\u00F3w\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 rodzaj grunt\\u00F3w\",\"Options\":null,\"Validation\":{\"MinLength\":1,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 1 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":6},{\"Name\":\"study_name\",\"Label\":\"Nazwa opracowania\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 nazw\\u0119 opracowania\",\"Options\":null,\"Validation\":{\"MinLength\":1,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 1 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":7},{\"Name\":\"study_author\",\"Label\":\"Autor\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 imi\\u0119 i nazwisko autora opracowania\",\"Options\":null,\"Validation\":{\"MinLength\":1,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 1 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":8}]" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Avatar",
                table: "Users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "TreeVotes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionDate",
                table: "TreeSubmissions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Legend",
                table: "TreeSubmissions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TreeSubmissions",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1500)",
                oldMaxLength: 1500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TreeSpecies",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1500)",
                oldMaxLength: 1500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "EmailVerificationTokens",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Communes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Communes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Communes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Communes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Communes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Communes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ApplicationTemplates",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Applications",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.UpdateData(
                table: "ApplicationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000005"),
                columns: new[] { "Fields", "Signature_Height", "Signature_Width", "Signature_Y" },
                values: new object[] { "[{\"Name\":\"justification\",\"Label\":\"Uzasadnienie wniosku\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 opisa\\u0107 dlaczego drzewo powinno zosta\\u0107 obj\\u0119te ochron\\u0105...\",\"Options\":null,\"Validation\":{\"MinLength\":50,\"MaxLength\":1000,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Uzasadnienie musi mie\\u0107 od 50 do 1000 znak\\u00F3w\"},\"HelpText\":\"Opisz walory przyrodnicze, historyczne lub krajobrazowe drzewa\",\"Order\":1},{\"Name\":\"estimated_care_cost\",\"Label\":\"Szacowany koszt rocznej opieki (z\\u0142)\",\"Type\":1,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"np. 500\",\"Options\":null,\"Validation\":{\"MinLength\":null,\"MaxLength\":null,\"Pattern\":null,\"Min\":0,\"Max\":10000,\"ValidationMessage\":\"Koszt musi by\\u0107 liczb\\u0105 od 0 do 10000\"},\"HelpText\":\"Przewidywany koszt opieki nad drzewem w ci\\u0105gu roku\",\"Order\":2},{\"Name\":\"responsible_person\",\"Label\":\"Osoba odpowiedzialna za opiek\\u0119\",\"Type\":0,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Imi\\u0119 i nazwisko\",\"Options\":null,\"Validation\":{\"MinLength\":3,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":null},\"HelpText\":null,\"Order\":3},{\"Name\":\"contact_phone\",\"Label\":\"Telefon kontaktowy osoby odpowiedzialnej\",\"Type\":3,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"\\u002B48 123 456 789\",\"Options\":null,\"Validation\":{\"MinLength\":null,\"MaxLength\":null,\"Pattern\":\"^\\\\\\u002B?[0-9\\\\s\\\\-\\\\(\\\\)]{9,15}$\",\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Numer telefonu musi zawiera\\u0107 9-15 cyfr\"},\"HelpText\":null,\"Order\":4},{\"Name\":\"care_agreement\",\"Label\":\"Zobowi\\u0105zuj\\u0119 si\\u0119 do sprawowania opieki nad drzewem\",\"Type\":9,\"IsRequired\":true,\"DefaultValue\":\"false\",\"Placeholder\":null,\"Options\":null,\"Validation\":null,\"HelpText\":\"Wymagane potwierdzenie zobowi\\u0105zania\",\"Order\":5}]", 80f, 120f, 235f });

            migrationBuilder.UpdateData(
                table: "ApplicationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000006"),
                columns: new[] { "Fields", "Signature_Height", "Signature_Width", "Signature_X", "Signature_Y" },
                values: new object[] { "[{\"Name\":\"justification\",\"Label\":\"Uzasadnienie wniosku\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 opisa\\u0107 dlaczego drzewo powinno zosta\\u0107 obj\\u0119te ochron\\u0105...\",\"Options\":null,\"Validation\":{\"MinLength\":50,\"MaxLength\":1000,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Uzasadnienie musi mie\\u0107 od 50 do 1000 znak\\u00F3w\"},\"HelpText\":\"Opisz walory przyrodnicze, historyczne lub krajobrazowe drzewa\",\"Order\":1},{\"Name\":\"location_plot\",\"Label\":\"Dzia\\u0142ka\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 dzia\\u0142k\\u0119 na kt\\u00F3rej znajduje si\\u0119 pomnik przyrody\",\"Options\":null,\"Validation\":{\"MinLength\":2,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 2 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":2},{\"Name\":\"location_district\",\"Label\":\"Obr\\u0119b ewidencyjny\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 obr\\u0119b ewidencyjny\",\"Options\":null,\"Validation\":{\"MinLength\":2,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 2 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":3},{\"Name\":\"record_keeping_unit\",\"Label\":\"Jednostka ewidencyjna\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 jednostk\\u0119 ewidencyjn\\u0105\",\"Options\":null,\"Validation\":{\"MinLength\":2,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 2 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":4},{\"Name\":\"ownership_form\",\"Label\":\"Forma w\\u0142asno\\u015Bci\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 form\\u0119 w\\u0142asno\\u015Bci\",\"Options\":null,\"Validation\":{\"MinLength\":2,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 2 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":5},{\"Name\":\"land_type\",\"Label\":\"Rodzaj grunt\\u00F3w\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 rodzaj grunt\\u00F3w\",\"Options\":null,\"Validation\":{\"MinLength\":2,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 2 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":6},{\"Name\":\"study_name\",\"Label\":\"Nazwa opracowania\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 nazw\\u0119 opracowania\",\"Options\":null,\"Validation\":{\"MinLength\":2,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 2 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":7},{\"Name\":\"study_author\",\"Label\":\"Autor\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 poda\\u0107 imi\\u0119 i nazwisko autora opracowania\",\"Options\":null,\"Validation\":{\"MinLength\":2,\"MaxLength\":100,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Tekst musi mie\\u0107 od 2 do 100 znak\\u00F3w\"},\"HelpText\":null,\"Order\":8}]", 10f, 10f, 10f, 10f });
        }
    }
}
