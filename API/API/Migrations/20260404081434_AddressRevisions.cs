using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddressRevisions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Barangays_Cities_CityId",
                table: "Barangays");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Cities_CityId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Regions_PsgcCode",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Provinces_PsgcCode",
                table: "Provinces");

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
                name: "ZipCode",
                table: "Barangays");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "Users",
                newName: "CityMunicipalityId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_CityId",
                table: "Users",
                newName: "IX_Users_CityMunicipalityId");

            migrationBuilder.RenameColumn(
                name: "PsgcCode",
                table: "Barangays",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_Barangays_PsgcCode",
                table: "Barangays",
                newName: "IX_Barangays_Code");

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

            migrationBuilder.CreateTable(
                name: "CitiesMunicipalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitiesMunicipalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CitiesMunicipalities_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_CitiesMunicipalities_ProvinceId",
                table: "CitiesMunicipalities",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Barangays_CitiesMunicipalities_CityId",
                table: "Barangays",
                column: "CityId",
                principalTable: "CitiesMunicipalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CitiesMunicipalities_CityMunicipalityId",
                table: "Users",
                column: "CityMunicipalityId",
                principalTable: "CitiesMunicipalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Barangays_CitiesMunicipalities_CityId",
                table: "Barangays");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_CitiesMunicipalities_CityMunicipalityId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "CitiesMunicipalities");

            migrationBuilder.DropIndex(
                name: "IX_Regions_Code",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Provinces_Code",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Provinces");

            migrationBuilder.RenameColumn(
                name: "CityMunicipalityId",
                table: "Users",
                newName: "CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_CityMunicipalityId",
                table: "Users",
                newName: "IX_Users_CityId");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Barangays",
                newName: "PsgcCode");

            migrationBuilder.RenameIndex(
                name: "IX_Barangays_Code",
                table: "Barangays",
                newName: "IX_Barangays_PsgcCode");

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
                name: "ZipCode",
                table: "Barangays",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false),
                    PsgcCode = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Cities_ProvinceId",
                table: "Cities",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_PsgcCode",
                table: "Cities",
                column: "PsgcCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Barangays_Cities_CityId",
                table: "Barangays",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Cities_CityId",
                table: "Users",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
