using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddressTableRevision_NewAddressAPI : Migration
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

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "CitiesMunicipalities");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CitiesMunicipalities");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "CitiesMunicipalities");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Barangays");

            migrationBuilder.AlterColumn<int>(
                name: "ProvinceId",
                table: "CitiesMunicipalities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "CitiesMunicipalities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CitiesMunicipalities_RegionId",
                table: "CitiesMunicipalities",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CitiesMunicipalities_Regions_RegionId",
                table: "CitiesMunicipalities",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CitiesMunicipalities_Regions_RegionId",
                table: "CitiesMunicipalities");

            migrationBuilder.DropIndex(
                name: "IX_CitiesMunicipalities_RegionId",
                table: "CitiesMunicipalities");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "CitiesMunicipalities");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Regions",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Provinces",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "ProvinceId",
                table: "CitiesMunicipalities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "CitiesMunicipalities",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "CitiesMunicipalities",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "CitiesMunicipalities",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Barangays",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

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
    }
}
