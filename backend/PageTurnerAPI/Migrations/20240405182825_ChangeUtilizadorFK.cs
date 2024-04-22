using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PageTurnerAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUtilizadorFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilizador_TipoUtilizador_tipoUtilizadortipoUtilId",
                table: "Utilizador");

            migrationBuilder.RenameColumn(
                name: "tipoUtilizadortipoUtilId",
                table: "Utilizador",
                newName: "tipoUtilizadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Utilizador_tipoUtilizadortipoUtilId",
                table: "Utilizador",
                newName: "IX_Utilizador_tipoUtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizador_TipoUtilizador_tipoUtilizadorId",
                table: "Utilizador",
                column: "tipoUtilizadorId",
                principalTable: "TipoUtilizador",
                principalColumn: "tipoUtilId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilizador_TipoUtilizador_tipoUtilizadorId",
                table: "Utilizador");

            migrationBuilder.RenameColumn(
                name: "tipoUtilizadorId",
                table: "Utilizador",
                newName: "tipoUtilizadortipoUtilId");

            migrationBuilder.RenameIndex(
                name: "IX_Utilizador_tipoUtilizadorId",
                table: "Utilizador",
                newName: "IX_Utilizador_tipoUtilizadortipoUtilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizador_TipoUtilizador_tipoUtilizadortipoUtilId",
                table: "Utilizador",
                column: "tipoUtilizadortipoUtilId",
                principalTable: "TipoUtilizador",
                principalColumn: "tipoUtilId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
