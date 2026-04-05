using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddressTables_CodeRemoveIsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Regions_Code",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Provinces_Code",
                table: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_CitiesMunicipalities_Code",
                table: "CitiesMunicipalities");

            migrationBuilder.DropIndex(
                name: "IX_Barangays_Code",
                table: "Barangays");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Code",
                table: "Regions",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_Code",
                table: "Provinces",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_CitiesMunicipalities_Code",
                table: "CitiesMunicipalities",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Barangays_Code",
                table: "Barangays",
                column: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Regions_Code",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Provinces_Code",
                table: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_CitiesMunicipalities_Code",
                table: "CitiesMunicipalities");

            migrationBuilder.DropIndex(
                name: "IX_Barangays_Code",
                table: "Barangays");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Code",
                table: "Regions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_Code",
                table: "Provinces",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CitiesMunicipalities_Code",
                table: "CitiesMunicipalities",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Barangays_Code",
                table: "Barangays",
                column: "Code",
                unique: true);
        }
    }
}
