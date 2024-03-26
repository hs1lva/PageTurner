using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PageTurnerAPI.Migrations
{
    /// <inheritdoc />
    public partial class add_one_to_many_relationships_avaliacao_livros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacaoLivro_Livro_livroId",
                table: "AvaliacaoLivro");

            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacaoLivro_Utilizador_utilizadorID",
                table: "AvaliacaoLivro");

            migrationBuilder.RenameColumn(
                name: "utilizadorID",
                table: "AvaliacaoLivro",
                newName: "UtilizadorId");

            migrationBuilder.RenameColumn(
                name: "nota",
                table: "AvaliacaoLivro",
                newName: "Nota");

            migrationBuilder.RenameColumn(
                name: "livroId",
                table: "AvaliacaoLivro",
                newName: "LivroId");

            migrationBuilder.RenameColumn(
                name: "dataAvaliacao",
                table: "AvaliacaoLivro",
                newName: "DataAvaliacao");

            migrationBuilder.RenameColumn(
                name: "avaliacaoId",
                table: "AvaliacaoLivro",
                newName: "AvaliacaoId");

            migrationBuilder.RenameIndex(
                name: "IX_AvaliacaoLivro_utilizadorID",
                table: "AvaliacaoLivro",
                newName: "IX_AvaliacaoLivro_UtilizadorId");

            migrationBuilder.RenameIndex(
                name: "IX_AvaliacaoLivro_livroId",
                table: "AvaliacaoLivro",
                newName: "IX_AvaliacaoLivro_LivroId");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacaoLivro_Livro_LivroId",
                table: "AvaliacaoLivro",
                column: "LivroId",
                principalTable: "Livro",
                principalColumn: "livroId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacaoLivro_Utilizador_UtilizadorId",
                table: "AvaliacaoLivro",
                column: "UtilizadorId",
                principalTable: "Utilizador",
                principalColumn: "utilizadorID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacaoLivro_Livro_LivroId",
                table: "AvaliacaoLivro");

            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacaoLivro_Utilizador_UtilizadorId",
                table: "AvaliacaoLivro");

            migrationBuilder.RenameColumn(
                name: "UtilizadorId",
                table: "AvaliacaoLivro",
                newName: "utilizadorID");

            migrationBuilder.RenameColumn(
                name: "Nota",
                table: "AvaliacaoLivro",
                newName: "nota");

            migrationBuilder.RenameColumn(
                name: "LivroId",
                table: "AvaliacaoLivro",
                newName: "livroId");

            migrationBuilder.RenameColumn(
                name: "DataAvaliacao",
                table: "AvaliacaoLivro",
                newName: "dataAvaliacao");

            migrationBuilder.RenameColumn(
                name: "AvaliacaoId",
                table: "AvaliacaoLivro",
                newName: "avaliacaoId");

            migrationBuilder.RenameIndex(
                name: "IX_AvaliacaoLivro_UtilizadorId",
                table: "AvaliacaoLivro",
                newName: "IX_AvaliacaoLivro_utilizadorID");

            migrationBuilder.RenameIndex(
                name: "IX_AvaliacaoLivro_LivroId",
                table: "AvaliacaoLivro",
                newName: "IX_AvaliacaoLivro_livroId");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacaoLivro_Livro_livroId",
                table: "AvaliacaoLivro",
                column: "livroId",
                principalTable: "Livro",
                principalColumn: "livroId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacaoLivro_Utilizador_utilizadorID",
                table: "AvaliacaoLivro",
                column: "utilizadorID",
                principalTable: "Utilizador",
                principalColumn: "utilizadorID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
