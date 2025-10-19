using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrzewaAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTemplatesForLlmIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Organization_Address",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organization_City",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organization_Correspondence_Address",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organization_Correspondence_City",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Organization_Correspondence_PoBox",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organization_Correspondence_PostalCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organization_Krs",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organization_Mail",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organization_Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organization_Phone",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organization_PostalCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organization_Regon",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ApplicationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000005"),
                columns: new[] { "Signature_Height", "Signature_Width", "Signature_Y", "Description", "Fields", "HtmlTemplate", "Name" },
                values: new object[] { 80f, 120f, 100f, "Wniosek Opisowy", "[{\"Name\":\"justification\",\"Label\":\"Uzasadnienie wniosku\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 opisa\\u0107 dlaczego drzewo powinno zosta\\u0107 obj\\u0119te ochron\\u0105...\",\"Options\":null,\"Validation\":{\"MinLength\":50,\"MaxLength\":1500,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Uzasadnienie musi mie\\u0107 od 50 do 1500 znak\\u00F3w\"},\"HelpText\":\"Opisz walory przyrodnicze, historyczne lub krajobrazowe drzewa\",\"Order\":1}]", "<!DOCTYPE html><html><meta charset=UTF-8><title>Wniosek o ustanowienie ochroną pomnikową drzew</title><style>body{font-family:'Times New Roman',Times,serif;}.container-flex{display:flex;flex-direction:row;justify-content:space-between}.sender{text-align:left}.sender p{padding:0}.date{text-align:right}.recipient{text-align:center;margin-bottom:15px;margin-top:25px}.recipient p{margin:2px 0;font-weight:700}.title{font-weight:700;text-align:center;margin-bottom:15px}.content{text-align:justify;text-justify:inter-word}.paragraph{text-indent:30px;line-height:1.65}.paragraph.first{margin-bottom:10px;text-indent:30px;font-style:italic;text-align:center}</style><div><div class=container-flex><div class=sender>{{sender}}</div><div class=date>{{user_city}}, {{generation_date}}r.</div></div><div class=recipient><p>{{commune_name}}<p>{{commune_address}}<p>{{commune_postal_code}} {{commune_city}}</div><div class=title>Wniosek o ustanowienie ochroną pomnikową drzew</div><div class=content><p class=\"paragraph first\"><span>Na podstawie art. 221 i art. 241 KPA, składam wniosek o opisanego niżej drzewa za pomnik przyrody w trybie art. 40-45 ustawy o ochronie przyrody.</span><p class=paragraph>{{justification}}<p class=paragraph>Załącznik<p class=paragraph>1. {{tree_images}}<p class=paragraph>2. {{tree_map_image}}</div></div>", "Standardowy szablon wniosku o rejestrację drzewa jako pomnika przyrody" });

            migrationBuilder.UpdateData(
                table: "ApplicationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000006"),
                columns: new[] { "Signature_Height", "Signature_Width", "Signature_Y", "Description", "Name" },
                values: new object[] { 80f, 120f, 100f, "Uznanie obiektu przyrodniczego za pomnik przyrody WS-13", "Szablon udostępniony przez gminę Kraków" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Organization_Address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Organization_City",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Organization_Correspondence_Address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Organization_Correspondence_City",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Organization_Correspondence_PoBox",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Organization_Correspondence_PostalCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Organization_Krs",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Organization_Mail",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Organization_Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Organization_Phone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Organization_PostalCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Organization_Regon",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "ApplicationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000005"),
                columns: new[] { "Description", "Fields", "HtmlTemplate", "Name", "Signature_Height", "Signature_Width", "Signature_Y" },
                values: new object[] { "Standardowy szablon wniosku o rejestrację drzewa jako pomnika przyrody", "[{\"Name\":\"justification\",\"Label\":\"Uzasadnienie wniosku\",\"Type\":6,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Prosz\\u0119 opisa\\u0107 dlaczego drzewo powinno zosta\\u0107 obj\\u0119te ochron\\u0105...\",\"Options\":null,\"Validation\":{\"MinLength\":50,\"MaxLength\":1500,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Uzasadnienie musi mie\\u0107 od 50 do 1500 znak\\u00F3w\"},\"HelpText\":\"Opisz walory przyrodnicze, historyczne lub krajobrazowe drzewa\",\"Order\":1},{\"Name\":\"estimated_care_cost\",\"Label\":\"Szacowany koszt rocznej opieki (z\\u0142)\",\"Type\":1,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"np. 500\",\"Options\":null,\"Validation\":{\"MinLength\":null,\"MaxLength\":null,\"Pattern\":null,\"Min\":0,\"Max\":10000,\"ValidationMessage\":\"Koszt musi by\\u0107 liczb\\u0105 od 0 do 10000\"},\"HelpText\":\"Przewidywany koszt opieki nad drzewem w ci\\u0105gu roku\",\"Order\":2},{\"Name\":\"responsible_person\",\"Label\":\"Osoba odpowiedzialna za opiek\\u0119\",\"Type\":0,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"Imi\\u0119 i nazwisko\",\"Options\":null,\"Validation\":{\"MinLength\":1,\"MaxLength\":200,\"Pattern\":null,\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Imi\\u0119 i nazwisko powinno zawiera\\u0107 od 1 do 200 znak\\u00F3w\"},\"HelpText\":null,\"Order\":3},{\"Name\":\"contact_phone\",\"Label\":\"Telefon kontaktowy osoby odpowiedzialnej\",\"Type\":3,\"IsRequired\":true,\"DefaultValue\":null,\"Placeholder\":\"\\u002B48 123 456 789\",\"Options\":null,\"Validation\":{\"MinLength\":9,\"MaxLength\":20,\"Pattern\":\"^\\\\\\u002B?[0-9\\\\s\\\\-\\\\(\\\\)]{9,15}$\",\"Min\":null,\"Max\":null,\"ValidationMessage\":\"Numer telefonu musi zawiera\\u0107 9-15 cyfr\"},\"HelpText\":null,\"Order\":4},{\"Name\":\"care_agreement\",\"Label\":\"Zobowi\\u0105zuj\\u0119 si\\u0119 do sprawowania opieki nad drzewem\",\"Type\":9,\"IsRequired\":true,\"DefaultValue\":\"false\",\"Placeholder\":null,\"Options\":null,\"Validation\":null,\"HelpText\":\"Wymagane potwierdzenie zobowi\\u0105zania\",\"Order\":5}]", "<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>Wniosek o rejestrację pomnika przyrody</title></head><body><h1>WNIOSEK O REJESTRACJĘ POMNIKA PRZYRODY</h1><div><h3>{{commune_name}}</h3><p>{{commune_address}}, {{commune_city}} {{commune_postal_code}}</p></div><div><h3>Dane wnioskodawcy:</h3><p>Imię i nazwisko: {{user_full_name}}</p><p>Adres: {{user_address}}, {{user_city}} {{user_postal_code}}</p><p>Telefon: {{user_phone}}</p><p>Email: {{user_email}}</p></div><div><h3>Dane drzewa:</h3><p>Gatunek: {{tree_species_polish}}</p><p>Obwód: {{tree_circumference}} cm</p><p>Wysokość: {{tree_height}} m</p><p>Wiek: {{tree_estimated_age}} lat</p><p>Stan: {{tree_condition}}</p></div><div><h3>Dodatkowe informacje:</h3><p>Uzasadnienie: {{justification}}</p><p>Przewidywany koszt opieki: {{estimated_care_cost}} zł/rok</p><p>Osoba odpowiedzialna: {{responsible_person}}</p><p>Telefon kontaktowy: {{contact_phone}}</p></div><div><p>Data: {{submission_date}}</p><p>Podpis: ................................<span style=position:relative; top:-5px; display:block;>{{user_full_name}}</span></p></div></body></html>", "Wniosek o rejestrację pomnika przyrody", 120f, 80f, 150f });

            migrationBuilder.UpdateData(
                table: "ApplicationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000006"),
                columns: new[] { "Description", "Name", "Signature_Height", "Signature_Width", "Signature_Y" },
                values: new object[] { "Standardowy szablon wniosku o rejestrację drzewa jako pomnika przyrody", "Uznanie obiektu przyrodniczego za pomnik przyrody WS-13", 120f, 80f, 150f });
        }
    }
}
