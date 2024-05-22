using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PageTurnerAPI.Migrations
{
    /// <inheritdoc />
    public partial class RefreshSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Limpar dados existentes nas tabelas específicas
            migrationBuilder.Sql("DELETE FROM TipoUtilizador");
            migrationBuilder.Sql("DELETE FROM ConteudoOfensivo");
            migrationBuilder.Sql("DELETE FROM EstadoComentario");
            migrationBuilder.Sql("DELETE FROM EstadoConta");
            migrationBuilder.Sql("DELETE FROM EstadoTroca");
            migrationBuilder.Sql("DELETE FROM TipoEstante");

            // Seed data for TipoUtilizador
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT TipoUtilizador ON;
                INSERT INTO TipoUtilizador (tipoUtilId, descricaoTipoUti) VALUES (1, 'Administrador');
                INSERT INTO TipoUtilizador (tipoUtilId, descricaoTipoUti) VALUES (2, 'Utilizador');
                SET IDENTITY_INSERT TipoUtilizador OFF;
            ");

            // Seed data for ConteudoOfensivo
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT ConteudoOfensivo ON;
                INSERT INTO ConteudoOfensivo (conteudoOfensivoId, especificacaoConteudoOfensivo) VALUES (1, 'palavraofensiva1');
                INSERT INTO ConteudoOfensivo (conteudoOfensivoId, especificacaoConteudoOfensivo) VALUES (2, 'palavraofensiva2');
                INSERT INTO ConteudoOfensivo (conteudoOfensivoId, especificacaoConteudoOfensivo) VALUES (3, 'palavraofensiva3');
                SET IDENTITY_INSERT ConteudoOfensivo OFF;
            ");

            // Seed data for EstadoComentario
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT EstadoComentario ON;
                INSERT INTO EstadoComentario (estadoComentarioId, descricaoEstadoComentario) VALUES (1, 'Ativo');
                INSERT INTO EstadoComentario (estadoComentarioId, descricaoEstadoComentario) VALUES (2, 'Removido');
                INSERT INTO EstadoComentario (estadoComentarioId, descricaoEstadoComentario) VALUES (3, 'Pendente');
                SET IDENTITY_INSERT EstadoComentario OFF;
            ");

            // Seed data for EstadoConta
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT EstadoConta ON;
                INSERT INTO EstadoConta (estadoContaId, descricaoEstadoConta) VALUES (1, 'Ativo');
                INSERT INTO EstadoConta (estadoContaId, descricaoEstadoConta) VALUES (2, 'Inativo');
                INSERT INTO EstadoConta (estadoContaId, descricaoEstadoConta) VALUES (3, 'Banido');
                SET IDENTITY_INSERT EstadoConta OFF;
            ");

            // Seed data for EstadoTroca
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT EstadoTroca ON;
                INSERT INTO EstadoTroca (estadoTrocaId, descricaoEstadoTroca) VALUES (1, 'Pendente');
                INSERT INTO EstadoTroca (estadoTrocaId, descricaoEstadoTroca) VALUES (2, 'Aceite');
                INSERT INTO EstadoTroca (estadoTrocaId, descricaoEstadoTroca) VALUES (3, 'Recusada');
                SET IDENTITY_INSERT EstadoTroca OFF;
            ");

            // Seed data for TipoEstante
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT TipoEstante ON;
                INSERT INTO TipoEstante (tipoEstanteId, descricaoTipoEstante) VALUES (1, 'desejos');
                INSERT INTO TipoEstante (tipoEstanteId, descricaoTipoEstante) VALUES (2, 'pessoal');
                INSERT INTO TipoEstante (tipoEstanteId, descricaoTipoEstante) VALUES (3, 'troca');
                SET IDENTITY_INSERT TipoEstante OFF;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverter os dados seed se necessário
            migrationBuilder.Sql("DELETE FROM TipoUtilizador");
            migrationBuilder.Sql("DELETE FROM ConteudoOfensivo");
            migrationBuilder.Sql("DELETE FROM EstadoComentario");
            migrationBuilder.Sql("DELETE FROM EstadoConta");
            migrationBuilder.Sql("DELETE FROM EstadoTroca");
            migrationBuilder.Sql("DELETE FROM TipoEstante");
        }
    }
    }

