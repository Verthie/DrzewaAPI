using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrzewaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedTreeSpeciesImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreeSpeciesImage_TreeSpecies_TreeSpeciesId",
                table: "TreeSpeciesImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreeSpeciesImage",
                table: "TreeSpeciesImage");

            migrationBuilder.RenameTable(
                name: "TreeSpeciesImage",
                newName: "TreeSpeciesImages");

            migrationBuilder.RenameColumn(
                name: "Lng",
                table: "TreeSubmissions",
                newName: "Location_Lng");

            migrationBuilder.RenameColumn(
                name: "Lat",
                table: "TreeSubmissions",
                newName: "Location_Lat");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "TreeSubmissions",
                newName: "Location_Address");

            migrationBuilder.RenameIndex(
                name: "IX_TreeSpeciesImage_TreeSpeciesId",
                table: "TreeSpeciesImages",
                newName: "IX_TreeSpeciesImages_TreeSpeciesId");

            migrationBuilder.AddColumn<string>(
                name: "SeasonalChanges_Autumn",
                table: "TreeSpecies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeasonalChanges_Spring",
                table: "TreeSpecies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeasonalChanges_Summer",
                table: "TreeSpecies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeasonalChanges_Winter",
                table: "TreeSpecies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Traits_Lifespan",
                table: "TreeSpecies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Traits_MaxHeight",
                table: "TreeSpecies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Traits_NativeToPoland",
                table: "TreeSpecies",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreeSpeciesImages",
                table: "TreeSpeciesImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TreeSpeciesImages_TreeSpecies_TreeSpeciesId",
                table: "TreeSpeciesImages",
                column: "TreeSpeciesId",
                principalTable: "TreeSpecies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreeSpeciesImages_TreeSpecies_TreeSpeciesId",
                table: "TreeSpeciesImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreeSpeciesImages",
                table: "TreeSpeciesImages");

            migrationBuilder.DropColumn(
                name: "SeasonalChanges_Autumn",
                table: "TreeSpecies");

            migrationBuilder.DropColumn(
                name: "SeasonalChanges_Spring",
                table: "TreeSpecies");

            migrationBuilder.DropColumn(
                name: "SeasonalChanges_Summer",
                table: "TreeSpecies");

            migrationBuilder.DropColumn(
                name: "SeasonalChanges_Winter",
                table: "TreeSpecies");

            migrationBuilder.DropColumn(
                name: "Traits_Lifespan",
                table: "TreeSpecies");

            migrationBuilder.DropColumn(
                name: "Traits_MaxHeight",
                table: "TreeSpecies");

            migrationBuilder.DropColumn(
                name: "Traits_NativeToPoland",
                table: "TreeSpecies");

            migrationBuilder.RenameTable(
                name: "TreeSpeciesImages",
                newName: "TreeSpeciesImage");

            migrationBuilder.RenameColumn(
                name: "Location_Lng",
                table: "TreeSubmissions",
                newName: "Lng");

            migrationBuilder.RenameColumn(
                name: "Location_Lat",
                table: "TreeSubmissions",
                newName: "Lat");

            migrationBuilder.RenameColumn(
                name: "Location_Address",
                table: "TreeSubmissions",
                newName: "Address");

            migrationBuilder.RenameIndex(
                name: "IX_TreeSpeciesImages_TreeSpeciesId",
                table: "TreeSpeciesImage",
                newName: "IX_TreeSpeciesImage_TreeSpeciesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreeSpeciesImage",
                table: "TreeSpeciesImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TreeSpeciesImage_TreeSpecies_TreeSpeciesId",
                table: "TreeSpeciesImage",
                column: "TreeSpeciesId",
                principalTable: "TreeSpecies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
