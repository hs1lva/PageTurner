using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PageTurnerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddComentarioLivro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComentarioLivroConteudoOfensivo_CommentLivro_comentarioscomentarioId",
                table: "ComentarioLivroConteudoOfensivo");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentLivro_EstadoComentario_estadoComentarioId",
                table: "CommentLivro");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentLivro_Livro_livroId",
                table: "CommentLivro");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentLivro_Utilizador_utilizadorID",
                table: "CommentLivro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentLivro",
                table: "CommentLivro");

            migrationBuilder.DropIndex(
                name: "IX_CommentLivro_utilizadorID",
                table: "CommentLivro");

            migrationBuilder.RenameTable(
                name: "CommentLivro",
                newName: "ComentarioLivro");

            migrationBuilder.RenameColumn(
                name: "comentarioscomentarioId",
                table: "ComentarioLivroConteudoOfensivo",
                newName: "comentariosComentarioId");

            migrationBuilder.RenameColumn(
                name: "utilizadorID",
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
                name: "IX_CommentLivro_livroId",
                table: "ComentarioLivro",
                newName: "IX_ComentarioLivro_LivroId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentLivro_estadoComentarioId",
                table: "ComentarioLivro",
                newName: "IX_ComentarioLivro_estadoComentarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComentarioLivro",
                table: "ComentarioLivro",
                column: "ComentarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComentarioLivro_EstadoComentario_estadoComentarioId",
                table: "ComentarioLivro",
                column: "estadoComentarioId",
                principalTable: "EstadoComentario",
                principalColumn: "estadoComentarioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComentarioLivro_Livro_LivroId",
                table: "ComentarioLivro",
                column: "LivroId",
                principalTable: "Livro",
                principalColumn: "livroId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComentarioLivroConteudoOfensivo_ComentarioLivro_comentariosComentarioId",
                table: "ComentarioLivroConteudoOfensivo",
                column: "comentariosComentarioId",
                principalTable: "ComentarioLivro",
                principalColumn: "ComentarioId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComentarioLivro_EstadoComentario_estadoComentarioId",
                table: "ComentarioLivro");

            migrationBuilder.DropForeignKey(
                name: "FK_ComentarioLivro_Livro_LivroId",
                table: "ComentarioLivro");

            migrationBuilder.DropForeignKey(
                name: "FK_ComentarioLivroConteudoOfensivo_ComentarioLivro_comentariosComentarioId",
                table: "ComentarioLivroConteudoOfensivo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComentarioLivro",
                table: "ComentarioLivro");

            migrationBuilder.RenameTable(
                name: "ComentarioLivro",
                newName: "CommentLivro");

            migrationBuilder.RenameColumn(
                name: "comentariosComentarioId",
                table: "ComentarioLivroConteudoOfensivo",
                newName: "comentarioscomentarioId");

            migrationBuilder.RenameColumn(
                name: "UtilizadorId",
                table: "CommentLivro",
                newName: "utilizadorID");

            migrationBuilder.RenameColumn(
                name: "LivroId",
                table: "CommentLivro",
                newName: "livroId");

            migrationBuilder.RenameColumn(
                name: "DataComentario",
                table: "CommentLivro",
                newName: "dataComentario");

            migrationBuilder.RenameColumn(
                name: "Comentario",
                table: "CommentLivro",
                newName: "comentario");

            migrationBuilder.RenameColumn(
                name: "ComentarioId",
                table: "CommentLivro",
                newName: "comentarioId");

            migrationBuilder.RenameIndex(
                name: "IX_ComentarioLivro_LivroId",
                table: "CommentLivro",
                newName: "IX_CommentLivro_livroId");

            migrationBuilder.RenameIndex(
                name: "IX_ComentarioLivro_estadoComentarioId",
                table: "CommentLivro",
                newName: "IX_CommentLivro_estadoComentarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentLivro",
                table: "CommentLivro",
                column: "comentarioId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentLivro_utilizadorID",
                table: "CommentLivro",
                column: "utilizadorID");

            migrationBuilder.AddForeignKey(
                name: "FK_ComentarioLivroConteudoOfensivo_CommentLivro_comentarioscomentarioId",
                table: "ComentarioLivroConteudoOfensivo",
                column: "comentarioscomentarioId",
                principalTable: "CommentLivro",
                principalColumn: "comentarioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLivro_EstadoComentario_estadoComentarioId",
                table: "CommentLivro",
                column: "estadoComentarioId",
                principalTable: "EstadoComentario",
                principalColumn: "estadoComentarioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLivro_Livro_livroId",
                table: "CommentLivro",
                column: "livroId",
                principalTable: "Livro",
                principalColumn: "livroId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLivro_Utilizador_utilizadorID",
                table: "CommentLivro",
                column: "utilizadorID",
                principalTable: "Utilizador",
                principalColumn: "utilizadorID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
