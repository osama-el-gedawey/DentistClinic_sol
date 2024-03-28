using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentistClinic.Migrations
{
    /// <inheritdoc />
    public partial class modifyv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TPlans_Teeth");

            migrationBuilder.AddColumn<int>(
                name: "TPlanId",
                table: "Teeth",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Teeth_TPlanId",
                table: "Teeth",
                column: "TPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teeth_Tplans_TPlanId",
                table: "Teeth",
                column: "TPlanId",
                principalTable: "Tplans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teeth_Tplans_TPlanId",
                table: "Teeth");

            migrationBuilder.DropIndex(
                name: "IX_Teeth_TPlanId",
                table: "Teeth");

            migrationBuilder.DropColumn(
                name: "TPlanId",
                table: "Teeth");

            migrationBuilder.CreateTable(
                name: "TPlans_Teeth",
                columns: table => new
                {
                    TplansId = table.Column<int>(type: "int", nullable: false),
                    ToothId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPlans_Teeth", x => new { x.TplansId, x.ToothId });
                    table.ForeignKey(
                        name: "FK_TPlans_Teeth_Teeth_ToothId",
                        column: x => x.ToothId,
                        principalTable: "Teeth",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TPlans_Teeth_Tplans_TplansId",
                        column: x => x.TplansId,
                        principalTable: "Tplans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TPlans_Teeth_ToothId",
                table: "TPlans_Teeth",
                column: "ToothId");
        }
    }
}
