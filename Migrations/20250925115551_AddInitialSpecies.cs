using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrzewaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialSpecies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TreeSpecies",
                columns: new[] { "Id", "SeasonalChanges_Autumn", "SeasonalChanges_Spring", "SeasonalChanges_Summer", "SeasonalChanges_Winter", "Traits_Lifespan", "Traits_MaxHeight", "Traits_NativeToPoland", "Description", "Family", "IdentificationGuide", "Images", "LatinName", "PolishName" },
                values: new object[] { new Guid("c6d5f2b5-bc4a-4f3d-9b68-13e2a62f3ed8"), "Liście żółto-brązowe, opadają późno w sezonie. Dojrzałe żołędzie opadają i są zbierane przez zwierzęta", "Młode liście jasno-zielone, często z czerwonawym nalotem. Kwitnienie w maju - kotki męskie i niewielkie kwiaty żeńskie", "Liście ciemno-zielone, gęsta korona dająca dużo cienia. Rozwijają się żołędzie", "Charakterystyczna sylwetka z grubym pniem i rozłożystymi gałęziami. Kora wyraźnie bruzdowna", "Ponad 1000 lat", 40, true, "Dąb szypułkowy to jeden z najważniejszych gatunków drzew w Polsce. Może żyć ponad 1000 lat i osiągać wysokość do 40 metrów. Jest symbolem siły, trwałości i mądrości w kulturze słowiańskiej. Drewno dębu było używane do budowy statków, domów i mebli przez wieki.", "Fagaceae", "[\"Li\\u015Bcie z wyra\\u017Anymi wci\\u0119ciami, bez szypu\\u0142ek lub z bardzo kr\\u00F3tkimi szypu\\u0142kami\",\"\\u017Bo\\u0142\\u0119dzie na d\\u0142ugich szypu\\u0142kach (2-8 cm), dojrzewaj\\u0105 jesieni\\u0105\",\"Kora szara, g\\u0142\\u0119boko bruzdowna u starych okaz\\u00F3w, g\\u0142adka u m\\u0142odych\",\"Korona szeroka, roz\\u0142o\\u017Cysta, charakterystyczny pokr\\u00F3j \\u0022parasola\\u0022\",\"P\\u0105ki skupione na ko\\u0144cach p\\u0119d\\u00F3w, jajowate, br\\u0105zowe\"]", null, "Quercus Robur", "Dąb szypułkowy" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TreeSpecies",
                keyColumn: "Id",
                keyValue: new Guid("c6d5f2b5-bc4a-4f3d-9b68-13e2a62f3ed8"));
        }
    }
}
