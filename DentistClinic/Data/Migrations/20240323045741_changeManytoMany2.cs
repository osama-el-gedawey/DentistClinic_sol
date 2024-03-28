using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentistClinic.Migrations
{
    /// <inheritdoc />
    public partial class changeManytoMany2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TPlans_Teeth_Tplans_TplanId",
                table: "TPlans_Teeth");

            migrationBuilder.RenameColumn(
                name: "TplanId",
                table: "TPlans_Teeth",
                newName: "TplansId");

            migrationBuilder.AddForeignKey(
                name: "FK_TPlans_Teeth_Tplans_TplansId",
                table: "TPlans_Teeth",
                column: "TplansId",
                principalTable: "Tplans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TPlans_Teeth_Tplans_TplansId",
                table: "TPlans_Teeth");

            migrationBuilder.RenameColumn(
                name: "TplansId",
                table: "TPlans_Teeth",
                newName: "TplanId");

            migrationBuilder.AddForeignKey(
                name: "FK_TPlans_Teeth_Tplans_TplanId",
                table: "TPlans_Teeth",
                column: "TplanId",
                principalTable: "Tplans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
