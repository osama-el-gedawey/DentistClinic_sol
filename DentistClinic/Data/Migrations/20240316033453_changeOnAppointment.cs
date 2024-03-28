using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentistClinic.Migrations
{
    /// <inheritdoc />
    public partial class changeOnAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Appointments",
                newName: "Start");

            migrationBuilder.AddColumn<DateOnly>(
                name: "End",
                table: "Appointments",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "End",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "Start",
                table: "Appointments",
                newName: "Date");
        }
    }
}
