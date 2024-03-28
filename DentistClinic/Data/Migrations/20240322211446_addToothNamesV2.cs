using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentistClinic.Migrations
{
    /// <inheritdoc />
    public partial class addToothNamesV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "toothNames",
                table: "Tplans");

            migrationBuilder.CreateTable(
                name: "Teeth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TplansId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teeth", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teeth_Tplans_TplansId",
                        column: x => x.TplansId,
                        principalTable: "Tplans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teeth_TplansId",
                table: "Teeth",
                column: "TplansId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Teeth");

            migrationBuilder.AddColumn<string>(
                name: "toothNames",
                table: "Tplans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
