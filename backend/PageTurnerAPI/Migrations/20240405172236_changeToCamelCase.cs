using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PageTurnerAPI.Migrations
{
    /// <inheritdoc />
    public partial class changeToCamelCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComentarioLivro_Livro_LivroId",
                table: "ComentarioLivro");

            migrationBuilder.DropForeignKey(
                name: "FK_ComentarioLivro_Utilizador_UtilizadorId",
                table: "ComentarioLivro");

            migrationBuilder.DropForeignKey(
                name: "FK_ComentarioLivroConteudoOfensivo_ComentarioLivro_ComentarioId",
                table: "ComentarioLivroConteudoOfensivo");

            migrationBuilder.DropForeignKey(
                name: "FK_ComentarioLivroConteudoOfensivo_ConteudoOfensivo_ConteudoOfensivoId",
                table: "ComentarioLivroConteudoOfensivo");

            migrationBuilder.RenameColumn(
                name: "ConteudoOfensivoId",
                table: "ComentarioLivroConteudoOfensivo",
                newName: "conteudoOfensivoId");

            migrationBuilder.RenameColumn(
                name: "ComentarioId",
                table: "ComentarioLivroConteudoOfensivo",
                newName: "comentarioId");

            migrationBuilder.RenameIndex(
                name: "IX_ComentarioLivroConteudoOfensivo_ConteudoOfensivoId",
                table: "ComentarioLivroConteudoOfensivo",
                newName: "IX_ComentarioLivroConteudoOfensivo_conteudoOfensivoId");

            migrationBuilder.RenameColumn(
                name: "UtilizadorId",
                table: "ComentarioLivro",
                newName: "utilizadorId");

            migrationBuilder.RenameColumn(
                name: "LivroId",
                table: "ComentarioLivro",
                newName: "livroId");

            migrationBuilder.RenameColumn(
                name: "DataComentario",
                table: "ComentarioLivro",
                newName: "dataComentario");

            migrationBuilder.RenameColumn(
                name: "Comentario",
                table: "ComentarioLivro",
                newName: "comentario");

            migrationBuilder.RenameColumn(
                name: "ComentarioId",
                table: "ComentarioLivro",
                newName: "comentarioId");

            migrationBuilder.RenameIndex(
                name: "IX_ComentarioLivro_UtilizadorId",
                table: "ComentarioLivro",
                newName: "IX_ComentarioLivro_utilizadorId");

            migrationBuilder.RenameIndex(
                name: "IX_ComentarioLivro_LivroId",
                table: "ComentarioLivro",
                newName: "IX_ComentarioLivro_livroId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComentarioLivro_Livro_livroId",
                table: "ComentarioLivro",
                column: "livroId",
                principalTable: "Livro",
                principalColumn: "livroId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComentarioLivro_Utilizador_utilizadorId",
                table: "ComentarioLivro",
                column: "utilizadorId",
                principalTable: "Utilizador",
                principalColumn: "utilizadorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComentarioLivroConteudoOfensivo_ComentarioLivro_comentarioId",
                table: "ComentarioLivroConteudoOfensivo",
                column: "comentarioId",
                principalTable: "ComentarioLivro",
                principalColumn: "comentarioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComentarioLivroConteudoOfensivo_ConteudoOfensivo_conteudoOfensivoId",
                table: "ComentarioLivroConteudoOfensivo",
                column: "conteudoOfensivoId",
                principalTable: "ConteudoOfensivo",
                principalColumn: "conteudoOfensivoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComentarioLivro_Livro_livroId",
                table: "ComentarioLivro");

            migrationBuilder.DropForeignKey(
                name: "FK_ComentarioLivro_Utilizador_utilizadorId",
                table: "ComentarioLivro");

            migrationBuilder.DropForeignKey(
                name: "FK_ComentarioLivroConteudoOfensivo_ComentarioLivro_comentarioId",
                table: "ComentarioLivroConteudoOfensivo");

            migrationBuilder.DropForeignKey(
                name: "FK_ComentarioLivroConteudoOfensivo_ConteudoOfensivo_conteudoOfensivoId",
                table: "ComentarioLivroConteudoOfensivo");

            migrationBuilder.RenameColumn(
                name: "conteudoOfensivoId",
                table: "ComentarioLivroConteudoOfensivo",
                newName: "ConteudoOfensivoId");

            migrationBuilder.RenameColumn(
                name: "comentarioId",
                table: "ComentarioLivroConteudoOfensivo",
                newName: "ComentarioId");

            migrationBuilder.RenameIndex(
                name: "IX_ComentarioLivroConteudoOfensivo_conteudoOfensivoId",
                table: "ComentarioLivroConteudoOfensivo",
                newName: "IX_ComentarioLivroConteudoOfensivo_ConteudoOfensivoId");

            migrationBuilder.RenameColumn(
                name: "utilizadorId",
                table: "ComentarioLivro",
                newName: "UtilizadorId");

            migrationBuilder.RenameColumn(
                name: "livroId",
                table: "ComentarioLivro",
                newName: "LivroId");

            migrationBuilder.RenameColumn(
                name: "dataComentario",
                table: "ComentarioLivro",
                newName: "DataComentario");

            migrationBuilder.RenameColumn(
                name: "comentario",
                table: "ComentarioLivro",
                newName: "Comentario");

            migrationBuilder.RenameColumn(
                name: "comentarioId",
                table: "ComentarioLivro",
                newName: "ComentarioId");

            migrationBuilder.RenameIndex(
                name: "IX_ComentarioLivro_utilizadorId",
                table: "ComentarioLivro",
                newName: "IX_ComentarioLivro_UtilizadorId");

            migrationBuilder.RenameIndex(
                name: "IX_ComentarioLivro_livroId",
                table: "ComentarioLivro",
                newName: "IX_ComentarioLivro_LivroId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComentarioLivro_Livro_LivroId",
                table: "ComentarioLivro",
                column: "LivroId",
                principalTable: "Livro",
                principalColumn: "livroId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComentarioLivro_Utilizador_UtilizadorId",
                table: "ComentarioLivro",
                column: "UtilizadorId",
                principalTable: "Utilizador",
                principalColumn: "utilizadorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComentarioLivroConteudoOfensivo_ComentarioLivro_ComentarioId",
                table: "ComentarioLivroConteudoOfensivo",
                column: "ComentarioId",
                principalTable: "ComentarioLivro",
                principalColumn: "ComentarioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComentarioLivroConteudoOfensivo_ConteudoOfensivo_ConteudoOfensivoId",
                table: "ComentarioLivroConteudoOfensivo",
                column: "ConteudoOfensivoId",
                principalTable: "ConteudoOfensivo",
                principalColumn: "conteudoOfensivoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
