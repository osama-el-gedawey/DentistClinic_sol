using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentistClinic.Migrations
{
    /// <inheritdoc />
    public partial class changeInModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "XrayPrescriptionImages");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "MedicalHistoryImages");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "AnalysisPrescriptionImages");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Xrays",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Medicines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Analysis",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Xrays");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Analysis");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "XrayPrescriptionImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "MedicalHistoryImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "AnalysisPrescriptionImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsReserved = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });
        }
    }
}
