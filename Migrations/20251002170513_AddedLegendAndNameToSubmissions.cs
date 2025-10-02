using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrzewaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedLegendAndNameToSubmissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Legend",
                table: "TreeSubmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TreeSubmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ApplicationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000006"),
                column: "HtmlTemplate",
                value: "<!DOCTYPE html><html><meta charset=UTF-8><style>body{font-family:Arial,sans-serif;font-size:12px}.header{text-align:left;margin:0 5px 20px 5px;font-size:12px}.title{margin:42px 5px 42px 5px}.title h1{text-align:center;font-size:22px;font-weight:300;margin:0}.title h2{text-align:center;font-size:15px;margin:0 0 12px 0}.title p{font-size:14px}table{border-collapse:collapse;margin:auto}td{border:1px solid #000;padding:4px 8px 8px 8px;vertical-align:top}.number-col{width:30px;text-align:center}.question-col{width:42%}.answer-col{width:55%}.footer{display:flex;justify-content:space-between;align-items:flex-start;margin:64px 5px 0 5px}.footer *{margin:0}.signature{display:flex;flex-direction:column;text-align:center;align-items:center;justify-content:center;position:relative;}.signature-text{font-size:10px}.signature-field{position:absolute;bottom:17px;}</style><div class=header>Załącznik do procedury WS-13</div><div class=title><h1>Wniosek</h1><h2>o uznanie obiektu przyrodniczego za pomnik przyrody</h2><p>na podstawie Art. 6 ust. 1 pkt 6, art. 40, art. 44 ustawy z dnia 16 kwietnia 2004 r. o ochronie przyrody.</div><table><tr><td class=number-col>1.<td class=question-col>Imię i nazwisko wnioskodawcy / nazwa wnioskodawcy<br>Adres / siedziba wnioskodawcy<td class=answer-col>{{user_full_name}}<br>{{user_address}}<br>{{user_city}}, {{user_postal_code}}<tr><td class=number-col>2.<td class=question-col>Nazwa i rodzaj pomnika przyrody<td class=answer-col>Nazwa polska: {{tree_species_polish}}<br>Nazwa łacińska: {{tree_species_latin}}<br>Rodzaj: drzewo<tr><td class=number-col>3.<td class=question-col>Określenie położenia geograficznego i administracyjnego pomnika przyrody (działka, obręb ewidencyjny, jednostka ewidencyjna)<td class=answer-col>Położenie geograficzne: {{location_lat}} lat, {{location_long}} long<br>Działka: {{location_plot}}<br>Obręb ewidencyjny: {{location_district}}<br>Jednostka ewidencyjna: {{record_keeping_unit}}<tr><td class=number-col>4.<td class=question-col>Wskazanie formy własności i rodzajów gruntów<td class=answer-col>Forma własności: {{ownership_form}}<br>Rodzaj gruntów: {{land_type}}<tr><td class=number-col>5.<td class=question-col>Wskazanie mapy obrazującej lokalizację pomnika przyrody<td class=answer-col><tr><td class=number-col>6.<td class=question-col>Krótki opis pomnika przyrody<br>- dla pomników przyrody żywej gatunek, wiek, pierśnica, wysokość, rozpiętość korony, stan zdrowotny,<br>- dla pomników przyrody nieżywej typ, rodzaj, wielkość źródła, wodospadu, głazu, jaskini itp.<td class=answer-col>Wiek: {{tree_estimated_age}}<br>Pierśnica: {{tree_circumference}} cm<br>Wysokość: {{tree_height}} m<br>Rozpiętość: {{tree_spread}}<br>Stan zdrowotny: {{tree_condition}}<tr><td class=number-col>7.<td class=question-col>Nazwa, autor opracowania potwierdzającego wartości przyrodnicze obiektu<td class=answer-col>Nazwa opracowania: {{study_name}}<br>Autor: {{study_author}}</table><div class=footer><p>{{user_city}}, dn. {{generation_date}}<div class=signature><span class=signature-field>{{user_full_name}}</span><p>..............................................<p class=signature-text><em>(podpis)</em></div></div></body></html>");

            migrationBuilder.UpdateData(
                table: "TreeSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000007"),
                columns: new[] { "Legend", "Name" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TreeSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000008"),
                columns: new[] { "Legend", "Name" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Legend",
                table: "TreeSubmissions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TreeSubmissions");

            migrationBuilder.UpdateData(
                table: "ApplicationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-000000000006"),
                column: "HtmlTemplate",
                value: "<!DOCTYPE html><html><meta charset=UTF-8><style>body{font-family:Arial,sans-serif;font-size:12px}.header{text-align:left;margin:0 5px 20px 5px;font-size:12px}.title{margin:42px 5px 42px 5px}.title h1{text-align:center;font-size:22px;font-weight:300;margin:0}.title h2{text-align:center;font-size:15px;margin:0 0 12px 0}.title p{font-size:14px}table{border-collapse:collapse;margin:auto}td{border:1px solid #000;padding:4px 8px 8px 8px;vertical-align:top}.number-col{width:30px;text-align:center}.question-col{width:42%}.answer-col{width:55%}.footer{display:flex;justify-content:space-between;align-items:flex-start;margin:64px 5px 0 5px}.footer *{margin:0}.signature{display:flex;flex-direction:column;text-align:center;align-items:center;justify-content:center;position:relative;}.signature-text{font-size:10px}.signature-field{position:absolute;bottom:17px;}</style><div class=header>Załącznik do procedury WS-13</div><div class=title><h1>Wniosek</h1><h2>o uznanie obiektu przyrodniczego za pomnik przyrody</h2><p>na podstawie Art. 6 ust. 1 pkt 6, art. 40, art. 44 ustawy z dnia 16 kwietnia 2004 r. o ochronie przyrody.</div><table><tr><td class=number-col>1.<td class=question-col>Imię i nazwisko wnioskodawcy / nazwa wnioskodawcy<br>Adres / siedziba wnioskodawcy<td class=answer-col>{{user_full_name}}<br>{{user_address}}<br>{{user_city}}, {{user_postal_code}}<tr><td class=number-col>2.<td class=question-col>Nazwa i rodzaj pomnika przyrody<td class=answer-col>Nazwa polska: {{tree_species_polish}}<br>Nazwa łacińska: {{tree_species_latin}}<br>Rodzaj: drzewo<tr><td class=number-col>3.<td class=question-col>Określenie położenia geograficznego i administracyjnego pomnika przyrody (działka, obręb ewidencyjny, jednostka ewidencyjna)<td class=answer-col>Położenie geograficzne: {{location_lat}} lat, {{location_long}} long<br>Działka: {{location_plot}}<br>Obręb ewidencyjny: {{location_district}}<br>Jednostka ewidencyjna: {{record_keeping_unit}}<tr><td class=number-col>4.<td class=question-col>Wskazanie formy własności i rodzajów gruntów<td class=answer-col>Forma własności: {{ownership_form}}<br>Rodzaj gruntów: {{land_type}}<tr><td class=number-col>5.<td class=question-col>Wskazanie mapy obrazującej lokalizację pomnika przyrody<td class=answer-col><tr><td class=number-col>6.<td class=question-col>Krótki opis pomnika przyrody<br>- dla pomników przyrody żywej gatunek, wiek, pierśnica, wysokość, rozpiętość korony, stan zdrowotny,<br>- dla pomników przyrody nieżywej typ, rodzaj, wielkość źródła, wodospadu, głazu, jaskini itp.<td class=answer-col>Wiek: {{tree_estimated_age}}<br>Pierśnica: {{tree_circumference}} cm<br>Wysokość: {{tree_height}} m<br>Rozpiętość:<br>Stan zdrowotny: {{tree_condition}}<tr><td class=number-col>7.<td class=question-col>Nazwa, autor opracowania potwierdzającego wartości przyrodnicze obiektu<td class=answer-col>Nazwa opracowania: {{study_name}}<br>Autor: {{study_author}}</table><div class=footer><p>{{user_city}}, dn. {{generation_date}}<div class=signature><span class=signature-field>{{user_full_name}}</span><p>..............................................<p class=signature-text><em>(podpis)</em></div></div></body></html>");
        }
    }
}
