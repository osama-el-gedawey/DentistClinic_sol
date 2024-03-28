using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentistClinic.Migrations
{
    /// <inheritdoc />
    public partial class contactMsgIsConfirmed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "ContactMsgs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "ContactMsgs");
        }
    }
}
