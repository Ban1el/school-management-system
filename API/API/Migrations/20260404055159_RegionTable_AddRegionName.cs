using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class RegionTable_AddRegionName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PsgcCode",
                table: "Regions",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "Regions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PsgcCode",
                table: "Provinces",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PsgcCode",
                table: "Cities",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PsgcCode",
                table: "Barangays",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_PsgcCode",
                table: "Regions",
                column: "PsgcCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_PsgcCode",
                table: "Provinces",
                column: "PsgcCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_PsgcCode",
                table: "Cities",
                column: "PsgcCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Barangays_PsgcCode",
                table: "Barangays",
                column: "PsgcCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Regions_PsgcCode",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Provinces_PsgcCode",
                table: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Cities_PsgcCode",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Barangays_PsgcCode",
                table: "Barangays");

            migrationBuilder.DropColumn(
                name: "PsgcCode",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "PsgcCode",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "PsgcCode",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "PsgcCode",
                table: "Barangays");
        }
    }
}
