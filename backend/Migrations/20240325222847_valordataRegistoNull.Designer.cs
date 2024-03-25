﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace PageTurnerAPI.Migrations
{
    [DbContext(typeof(PageTurnerContext))]
    [Migration("20240325222847_valordataRegistoNull")]
    partial class valordataRegistoNull
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ComentarioLivroConteudoOfensivo", b =>
                {
                    b.Property<int>("comentarioscomentarioId")
                        .HasColumnType("int");

                    b.Property<int>("conteudoOfensivoId")
                        .HasColumnType("int");

                    b.HasKey("comentarioscomentarioId", "conteudoOfensivoId");

                    b.HasIndex("conteudoOfensivoId");

                    b.ToTable("ComentarioLivroConteudoOfensivo");
                });

            modelBuilder.Entity("backend.Models.AutorLivro", b =>
                {
                    b.Property<int>("autorLivroId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("autorLivroId"));

                    b.Property<string>("nomeAutorNome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("autorLivroId");

                    b.ToTable("AutorLivro");
                });

            modelBuilder.Entity("backend.Models.AvaliacaoLivro", b =>
                {
                    b.Property<int>("avaliacaoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("avaliacaoId"));

                    b.Property<DateTime>("dataAvaliacao")
                        .HasColumnType("datetime2");

                    b.Property<int>("livroId")
                        .HasColumnType("int");

                    b.Property<int>("nota")
                        .HasColumnType("int");

                    b.Property<int>("utilizadorID")
                        .HasColumnType("int");

                    b.HasKey("avaliacaoId");

                    b.HasIndex("livroId");

                    b.HasIndex("utilizadorID");

                    b.ToTable("AvaliacaoLivro");
                });

            modelBuilder.Entity("backend.Models.Cidade", b =>
                {
                    b.Property<int>("cidadeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("cidadeId"));

                    b.Property<string>("nomeCidade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("paisCidadepaisId")
                        .HasColumnType("int");

                    b.HasKey("cidadeId");

                    b.HasIndex("paisCidadepaisId");

                    b.ToTable("Cidade");
                });

            modelBuilder.Entity("backend.Models.ComentarioLivro", b =>
                {
                    b.Property<int>("comentarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("comentarioId"));

                    b.Property<string>("comentario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("dataComentario")
                        .HasColumnType("datetime2");

                    b.Property<int>("estadoComentarioId")
                        .HasColumnType("int");

                    b.Property<int>("livroId")
                        .HasColumnType("int");

                    b.Property<int>("utilizadorID")
                        .HasColumnType("int");

                    b.HasKey("comentarioId");

                    b.HasIndex("estadoComentarioId");

                    b.HasIndex("livroId");

                    b.HasIndex("utilizadorID");

                    b.ToTable("CommentLivro");
                });

            modelBuilder.Entity("backend.Models.ConteudoOfensivo", b =>
                {
                    b.Property<int>("conteudoOfensivoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("conteudoOfensivoId"));

                    b.Property<string>("especificacaoConteudoOfensivo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("conteudoOfensivoId");

                    b.ToTable("ConteudoOfensivo");
                });

            modelBuilder.Entity("backend.Models.EstadoComentario", b =>
                {
                    b.Property<int>("estadoComentarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("estadoComentarioId"));

                    b.Property<string>("descricaoEstadoComentario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("estadoComentarioId");

                    b.ToTable("EstadoComentario");
                });

            modelBuilder.Entity("backend.Models.EstadoConta", b =>
                {
                    b.Property<int>("estadoContaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("estadoContaId"));

                    b.Property<string>("descricaoEstadoConta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("estadoContaId");

                    b.ToTable("EstadoConta");
                });

            modelBuilder.Entity("backend.Models.EstadoTroca", b =>
                {
                    b.Property<int>("estadoTrocaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("estadoTrocaId"));

                    b.Property<string>("descricaoEstadoTroca")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("estadoTrocaId");

                    b.ToTable("EstadoTroca");
                });

            modelBuilder.Entity("backend.Models.Estante", b =>
                {
                    b.Property<int>("estanteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("estanteId"));

                    b.Property<int>("livroId")
                        .HasColumnType("int");

                    b.Property<int>("tipoEstanteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ultimaAtualizacao")
                        .HasColumnType("datetime2");

                    b.Property<int>("utilizadorID")
                        .HasColumnType("int");

                    b.HasKey("estanteId");

                    b.HasIndex("livroId");

                    b.HasIndex("tipoEstanteId");

                    b.HasIndex("utilizadorID");

                    b.ToTable("Estante");
                });

            modelBuilder.Entity("backend.Models.GeneroLivro", b =>
                {
                    b.Property<int>("generoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("generoId"));

                    b.Property<string>("descricaoGenero")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("generoId");

                    b.ToTable("GeneroLivro");
                });

            modelBuilder.Entity("backend.Models.Livro", b =>
                {
                    b.Property<int>("livroId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("livroId"));

                    b.Property<int>("anoPrimeiraPublicacao")
                        .HasColumnType("int");

                    b.Property<int>("autorLivroId")
                        .HasColumnType("int");

                    b.Property<int>("generoLivrogeneroId")
                        .HasColumnType("int");

                    b.Property<int>("idiomaOriginalLivro")
                        .HasColumnType("int");

                    b.Property<string>("tituloLivro")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("livroId");

                    b.HasIndex("autorLivroId");

                    b.HasIndex("generoLivrogeneroId");

                    b.ToTable("Livro");
                });

            modelBuilder.Entity("backend.Models.Pais", b =>
                {
                    b.Property<int>("paisId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("paisId"));

                    b.Property<string>("nomePais")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("paisId");

                    b.ToTable("Pais");
                });

            modelBuilder.Entity("backend.Models.TipoEstante", b =>
                {
                    b.Property<int>("tipoEstanteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("tipoEstanteId"));

                    b.Property<string>("descricaoTipoEstante")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("tipoEstanteId");

                    b.ToTable("TipoEstante");
                });

            modelBuilder.Entity("backend.Models.TipoUtilizador", b =>
                {
                    b.Property<int>("tipoUtilId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("tipoUtilId"));

                    b.Property<string>("descricaoTipoUti")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("tipoUtilId");

                    b.ToTable("TipoUtilizador");
                });

            modelBuilder.Entity("backend.Models.Troca", b =>
                {
                    b.Property<int>("trocaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("trocaId"));

                    b.Property<DateTime?>("dataAceiteTroca")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("dataPedidoTroca")
                        .HasColumnType("datetime2");

                    b.Property<int>("estadoTrocaId")
                        .HasColumnType("int");

                    b.Property<int>("estanteId")
                        .HasColumnType("int");

                    b.HasKey("trocaId");

                    b.HasIndex("estadoTrocaId");

                    b.HasIndex("estanteId");

                    b.ToTable("Troca");
                });

            modelBuilder.Entity("backend.Models.Utilizador", b =>
                {
                    b.Property<int>("utilizadorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("utilizadorID"));

                    b.Property<string>("apelido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("dataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dataRegisto")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("estadoContaId")
                        .HasColumnType("int");

                    b.Property<string>("fotoPerfil")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("notficacaoAceiteTroca")
                        .HasColumnType("bit");

                    b.Property<bool>("notficacaoCorrespondencia")
                        .HasColumnType("bit");

                    b.Property<bool>("notficacaoPedidoTroca")
                        .HasColumnType("bit");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("tipoUtilizadortipoUtilId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ultimologin")
                        .HasColumnType("datetime2");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("utilizadorID");

                    b.HasIndex("estadoContaId");

                    b.HasIndex("tipoUtilizadortipoUtilId");

                    b.ToTable("Utilizador");
                });

            modelBuilder.Entity("ComentarioLivroConteudoOfensivo", b =>
                {
                    b.HasOne("backend.Models.ComentarioLivro", null)
                        .WithMany()
                        .HasForeignKey("comentarioscomentarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Models.ConteudoOfensivo", null)
                        .WithMany()
                        .HasForeignKey("conteudoOfensivoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("backend.Models.AvaliacaoLivro", b =>
                {
                    b.HasOne("backend.Models.Livro", "livro")
                        .WithMany()
                        .HasForeignKey("livroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Models.Utilizador", "utilizador")
                        .WithMany()
                        .HasForeignKey("utilizadorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("livro");

                    b.Navigation("utilizador");
                });

            modelBuilder.Entity("backend.Models.Cidade", b =>
                {
                    b.HasOne("backend.Models.Pais", "paisCidade")
                        .WithMany()
                        .HasForeignKey("paisCidadepaisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("paisCidade");
                });

            modelBuilder.Entity("backend.Models.ComentarioLivro", b =>
                {
                    b.HasOne("backend.Models.EstadoComentario", "estadoComentario")
                        .WithMany()
                        .HasForeignKey("estadoComentarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Models.Livro", "livro")
                        .WithMany()
                        .HasForeignKey("livroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Models.Utilizador", "utilizador")
                        .WithMany()
                        .HasForeignKey("utilizadorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("estadoComentario");

                    b.Navigation("livro");

                    b.Navigation("utilizador");
                });

            modelBuilder.Entity("backend.Models.Estante", b =>
                {
                    b.HasOne("backend.Models.Livro", "livro")
                        .WithMany()
                        .HasForeignKey("livroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Models.TipoEstante", "tipoEstante")
                        .WithMany()
                        .HasForeignKey("tipoEstanteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Models.Utilizador", "utilizador")
                        .WithMany()
                        .HasForeignKey("utilizadorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("livro");

                    b.Navigation("tipoEstante");

                    b.Navigation("utilizador");
                });

            modelBuilder.Entity("backend.Models.Livro", b =>
                {
                    b.HasOne("backend.Models.AutorLivro", "autorLivro")
                        .WithMany()
                        .HasForeignKey("autorLivroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Models.GeneroLivro", "generoLivro")
                        .WithMany()
                        .HasForeignKey("generoLivrogeneroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("autorLivro");

                    b.Navigation("generoLivro");
                });

            modelBuilder.Entity("backend.Models.Troca", b =>
                {
                    b.HasOne("backend.Models.EstadoTroca", "estadoTroca")
                        .WithMany()
                        .HasForeignKey("estadoTrocaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Models.Estante", "estanteId2")
                        .WithMany()
                        .HasForeignKey("estanteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("estadoTroca");

                    b.Navigation("estanteId2");
                });

            modelBuilder.Entity("backend.Models.Utilizador", b =>
                {
                    b.HasOne("backend.Models.EstadoConta", "estadoConta")
                        .WithMany()
                        .HasForeignKey("estadoContaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Models.TipoUtilizador", "tipoUtilizador")
                        .WithMany()
                        .HasForeignKey("tipoUtilizadortipoUtilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("estadoConta");

                    b.Navigation("tipoUtilizador");
                });
#pragma warning restore 612, 618
        }
    }
}
