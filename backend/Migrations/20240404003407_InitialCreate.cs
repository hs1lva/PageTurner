using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PageTurnerAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutorLivro",
                columns: table => new
                {
                    autorLivroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nomeAutorNome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutorLivro", x => x.autorLivroId);
                });

            migrationBuilder.CreateTable(
                name: "ConteudoOfensivo",
                columns: table => new
                {
                    conteudoOfensivoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    especificacaoConteudoOfensivo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConteudoOfensivo", x => x.conteudoOfensivoId);
                });

            migrationBuilder.CreateTable(
                name: "EstadoComentario",
                columns: table => new
                {
                    estadoComentarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descricaoEstadoComentario = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoComentario", x => x.estadoComentarioId);
                });

            migrationBuilder.CreateTable(
                name: "EstadoConta",
                columns: table => new
                {
                    estadoContaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descricaoEstadoConta = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoConta", x => x.estadoContaId);
                });

            migrationBuilder.CreateTable(
                name: "EstadoTroca",
                columns: table => new
                {
                    estadoTrocaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descricaoEstadoTroca = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoTroca", x => x.estadoTrocaId);
                });

            migrationBuilder.CreateTable(
                name: "GeneroLivro",
                columns: table => new
                {
                    generoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descricaoGenero = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneroLivro", x => x.generoId);
                });

            migrationBuilder.CreateTable(
                name: "Pais",
                columns: table => new
                {
                    paisId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nomePais = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pais", x => x.paisId);
                });

            migrationBuilder.CreateTable(
                name: "TipoEstante",
                columns: table => new
                {
                    tipoEstanteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descricaoTipoEstante = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoEstante", x => x.tipoEstanteId);
                });

            migrationBuilder.CreateTable(
                name: "TipoUtilizador",
                columns: table => new
                {
                    tipoUtilId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descricaoTipoUti = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoUtilizador", x => x.tipoUtilId);
                });

            migrationBuilder.CreateTable(
                name: "Livro",
                columns: table => new
                {
                    livroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tituloLivro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    anoPrimeiraPublicacao = table.Column<int>(type: "int", nullable: false),
                    idiomaOriginalLivro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    autorLivroId = table.Column<int>(type: "int", nullable: false),
                    generoLivrogeneroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livro", x => x.livroId);
                    table.ForeignKey(
                        name: "FK_Livro_AutorLivro_autorLivroId",
                        column: x => x.autorLivroId,
                        principalTable: "AutorLivro",
                        principalColumn: "autorLivroId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Livro_GeneroLivro_generoLivrogeneroId",
                        column: x => x.generoLivrogeneroId,
                        principalTable: "GeneroLivro",
                        principalColumn: "generoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cidade",
                columns: table => new
                {
                    cidadeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nomeCidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paisCidadepaisId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cidade", x => x.cidadeId);
                    table.ForeignKey(
                        name: "FK_Cidade_Pais_paisCidadepaisId",
                        column: x => x.paisCidadepaisId,
                        principalTable: "Pais",
                        principalColumn: "paisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Utilizador",
                columns: table => new
                {
                    utilizadorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apelido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fotoPerfil = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dataRegisto = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ultimologin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    notficacaoPedidoTroca = table.Column<bool>(type: "bit", nullable: false),
                    notficacaoAceiteTroca = table.Column<bool>(type: "bit", nullable: false),
                    notficacaoCorrespondencia = table.Column<bool>(type: "bit", nullable: false),
                    tipoUtilizadortipoUtilId = table.Column<int>(type: "int", nullable: false),
                    estadoContaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizador", x => x.utilizadorID);
                    table.ForeignKey(
                        name: "FK_Utilizador_EstadoConta_estadoContaId",
                        column: x => x.estadoContaId,
                        principalTable: "EstadoConta",
                        principalColumn: "estadoContaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Utilizador_TipoUtilizador_tipoUtilizadortipoUtilId",
                        column: x => x.tipoUtilizadortipoUtilId,
                        principalTable: "TipoUtilizador",
                        principalColumn: "tipoUtilId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AvaliacaoLivro",
                columns: table => new
                {
                    AvaliacaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nota = table.Column<int>(type: "int", nullable: false),
                    DataAvaliacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UtilizadorId = table.Column<int>(type: "int", nullable: false),
                    LivroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaliacaoLivro", x => x.AvaliacaoId);
                    table.ForeignKey(
                        name: "FK_AvaliacaoLivro_Livro_LivroId",
                        column: x => x.LivroId,
                        principalTable: "Livro",
                        principalColumn: "livroId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvaliacaoLivro_Utilizador_UtilizadorId",
                        column: x => x.UtilizadorId,
                        principalTable: "Utilizador",
                        principalColumn: "utilizadorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComentarioLivro",
                columns: table => new
                {
                    ComentarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataComentario = table.Column<DateTime>(type: "datetime2", nullable: false),
                    estadoComentarioId = table.Column<int>(type: "int", nullable: false),
                    UtilizadorId = table.Column<int>(type: "int", nullable: false),
                    LivroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComentarioLivro", x => x.ComentarioId);
                    table.ForeignKey(
                        name: "FK_ComentarioLivro_EstadoComentario_estadoComentarioId",
                        column: x => x.estadoComentarioId,
                        principalTable: "EstadoComentario",
                        principalColumn: "estadoComentarioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComentarioLivro_Livro_LivroId",
                        column: x => x.LivroId,
                        principalTable: "Livro",
                        principalColumn: "livroId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComentarioLivro_Utilizador_UtilizadorId",
                        column: x => x.UtilizadorId,
                        principalTable: "Utilizador",
                        principalColumn: "utilizadorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Estante",
                columns: table => new
                {
                    estanteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ultimaAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tipoEstanteId = table.Column<int>(type: "int", nullable: false),
                    utilizadorID = table.Column<int>(type: "int", nullable: false),
                    livroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estante", x => x.estanteId);
                    table.ForeignKey(
                        name: "FK_Estante_Livro_livroId",
                        column: x => x.livroId,
                        principalTable: "Livro",
                        principalColumn: "livroId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Estante_TipoEstante_tipoEstanteId",
                        column: x => x.tipoEstanteId,
                        principalTable: "TipoEstante",
                        principalColumn: "tipoEstanteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Estante_Utilizador_utilizadorID",
                        column: x => x.utilizadorID,
                        principalTable: "Utilizador",
                        principalColumn: "utilizadorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComentarioLivroConteudoOfensivo",
                columns: table => new
                {
                    ComentarioId = table.Column<int>(type: "int", nullable: false),
                    ConteudoOfensivoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComentarioLivroConteudoOfensivo", x => new { x.ComentarioId, x.ConteudoOfensivoId });
                    table.ForeignKey(
                        name: "FK_ComentarioLivroConteudoOfensivo_ComentarioLivro_ComentarioId",
                        column: x => x.ComentarioId,
                        principalTable: "ComentarioLivro",
                        principalColumn: "ComentarioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComentarioLivroConteudoOfensivo_ConteudoOfensivo_ConteudoOfensivoId",
                        column: x => x.ConteudoOfensivoId,
                        principalTable: "ConteudoOfensivo",
                        principalColumn: "conteudoOfensivoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Troca",
                columns: table => new
                {
                    trocaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dataPedidoTroca = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dataAceiteTroca = table.Column<DateTime>(type: "datetime2", nullable: true),
                    estanteId = table.Column<int>(type: "int", nullable: false),
                    estadoTrocaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Troca", x => x.trocaId);
                    table.ForeignKey(
                        name: "FK_Troca_EstadoTroca_estadoTrocaId",
                        column: x => x.estadoTrocaId,
                        principalTable: "EstadoTroca",
                        principalColumn: "estadoTrocaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Troca_Estante_estanteId",
                        column: x => x.estanteId,
                        principalTable: "Estante",
                        principalColumn: "estanteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacaoLivro_LivroId",
                table: "AvaliacaoLivro",
                column: "LivroId");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacaoLivro_UtilizadorId",
                table: "AvaliacaoLivro",
                column: "UtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cidade_paisCidadepaisId",
                table: "Cidade",
                column: "paisCidadepaisId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentarioLivro_estadoComentarioId",
                table: "ComentarioLivro",
                column: "estadoComentarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentarioLivro_LivroId",
                table: "ComentarioLivro",
                column: "LivroId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentarioLivro_UtilizadorId",
                table: "ComentarioLivro",
                column: "UtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentarioLivroConteudoOfensivo_ConteudoOfensivoId",
                table: "ComentarioLivroConteudoOfensivo",
                column: "ConteudoOfensivoId");

            migrationBuilder.CreateIndex(
                name: "IX_Estante_livroId",
                table: "Estante",
                column: "livroId");

            migrationBuilder.CreateIndex(
                name: "IX_Estante_tipoEstanteId",
                table: "Estante",
                column: "tipoEstanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Estante_utilizadorID",
                table: "Estante",
                column: "utilizadorID");

            migrationBuilder.CreateIndex(
                name: "IX_Livro_autorLivroId",
                table: "Livro",
                column: "autorLivroId");

            migrationBuilder.CreateIndex(
                name: "IX_Livro_generoLivrogeneroId",
                table: "Livro",
                column: "generoLivrogeneroId");

            migrationBuilder.CreateIndex(
                name: "IX_Troca_estadoTrocaId",
                table: "Troca",
                column: "estadoTrocaId");

            migrationBuilder.CreateIndex(
                name: "IX_Troca_estanteId",
                table: "Troca",
                column: "estanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizador_estadoContaId",
                table: "Utilizador",
                column: "estadoContaId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizador_tipoUtilizadortipoUtilId",
                table: "Utilizador",
                column: "tipoUtilizadortipoUtilId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvaliacaoLivro");

            migrationBuilder.DropTable(
                name: "Cidade");

            migrationBuilder.DropTable(
                name: "ComentarioLivroConteudoOfensivo");

            migrationBuilder.DropTable(
                name: "Troca");

            migrationBuilder.DropTable(
                name: "Pais");

            migrationBuilder.DropTable(
                name: "ComentarioLivro");

            migrationBuilder.DropTable(
                name: "ConteudoOfensivo");

            migrationBuilder.DropTable(
                name: "EstadoTroca");

            migrationBuilder.DropTable(
                name: "Estante");

            migrationBuilder.DropTable(
                name: "EstadoComentario");

            migrationBuilder.DropTable(
                name: "Livro");

            migrationBuilder.DropTable(
                name: "TipoEstante");

            migrationBuilder.DropTable(
                name: "Utilizador");

            migrationBuilder.DropTable(
                name: "AutorLivro");

            migrationBuilder.DropTable(
                name: "GeneroLivro");

            migrationBuilder.DropTable(
                name: "EstadoConta");

            migrationBuilder.DropTable(
                name: "TipoUtilizador");
        }
    }
}
