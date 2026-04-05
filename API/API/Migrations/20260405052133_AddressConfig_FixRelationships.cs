using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddressConfig_FixRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CitiesMunicipalities_Provinces_ProvinceId",
                table: "CitiesMunicipalities");

            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_Regions_RegionId",
                table: "Provinces");

            migrationBuilder.AddForeignKey(
                name: "FK_CitiesMunicipalities_Provinces_ProvinceId",
                table: "CitiesMunicipalities",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_Regions_RegionId",
                table: "Provinces",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CitiesMunicipalities_Provinces_ProvinceId",
                table: "CitiesMunicipalities");

            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_Regions_RegionId",
                table: "Provinces");

            migrationBuilder.AddForeignKey(
                name: "FK_CitiesMunicipalities_Provinces_ProvinceId",
                table: "CitiesMunicipalities",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_Regions_RegionId",
                table: "Provinces",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
