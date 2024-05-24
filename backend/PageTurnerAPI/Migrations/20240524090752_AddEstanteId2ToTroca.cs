using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PageTurnerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEstanteId2ToTroca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Troca_Estante_estanteId",
                table: "Troca");

            migrationBuilder.AddColumn<int>(
                name: "estanteId2",
                table: "Troca",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Troca_estanteId2",
                table: "Troca",
                column: "estanteId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Troca_Estante_estanteId",
                table: "Troca",
                column: "estanteId",
                principalTable: "Estante",
                principalColumn: "estanteId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Troca_Estante_estanteId2",
                table: "Troca",
                column: "estanteId2",
                principalTable: "Estante",
                principalColumn: "estanteId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Troca_Estante_estanteId",
                table: "Troca");

            migrationBuilder.DropForeignKey(
                name: "FK_Troca_Estante_estanteId2",
                table: "Troca");

            migrationBuilder.DropIndex(
                name: "IX_Troca_estanteId2",
                table: "Troca");

            migrationBuilder.DropColumn(
                name: "estanteId2",
                table: "Troca");

            migrationBuilder.AddForeignKey(
                name: "FK_Troca_Estante_estanteId",
                table: "Troca",
                column: "estanteId",
                principalTable: "Estante",
                principalColumn: "estanteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
