
using backend.Models;
using Microsoft.EntityFrameworkCore;

public class PageTurnerContext:DbContext{
    public PageTurnerContext(DbContextOptions<PageTurnerContext> options) : base(options)
    {

    }

    //escrever aqui as tabelas, sempre que fazemos uma migraçao de BD será visto aqui e só depois irá aos modelos.
    public DbSet<AutorLivro> AutorLivro { get; set; }
    public DbSet<AvaliacaoLivro> AvaliacaoLivro { get; set; }
    public DbSet<Cidade> Cidade { get; set; }
    public DbSet<ComentarioLivro> ComentarioLivro { get; set; }
    public DbSet<ConteudoOfensivo> ConteudoOfensivo { get; set; }
    public DbSet<EstadoComentario> EstadoComentario { get; set; }
    public DbSet<ComentarioLivroConteudoOfensivo> ComentarioLivroConteudoOfensivo { get; set; }
    public DbSet<EstadoConta> EstadoConta { get; set; }
    public DbSet<EstadoTroca> EstadoTroca { get; set; }
    public DbSet<Estante> Estante { get; set; }
    public DbSet<GeneroLivro> GeneroLivro { get; set; }
    public DbSet<Livro> Livro { get; set; }
    public DbSet<Pais> Pais { get; set; }
    public DbSet<TipoEstante> TipoEstante { get; set; }
    public DbSet<TipoUtilizador> TipoUtilizador { get; set; }
    public DbSet<Troca> Troca { get; set; }
    public DbSet<Utilizador> Utilizador { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração de chave primária composta
        modelBuilder.Entity<ComentarioLivroConteudoOfensivo>()
            .HasKey(c => new { c.ComentarioId, c.ConteudoOfensivoId });

        modelBuilder.Entity<ComentarioLivroConteudoOfensivo>()
            .HasOne(cco => cco.ComentarioLivro)
            .WithMany(cl => cl.ComentarioConteudoOfensivo)
            .HasForeignKey(cco => cco.ComentarioId);

        modelBuilder.Entity<ComentarioLivroConteudoOfensivo>()
            .HasOne(cco => cco.ConteudoOfensivo)
            .WithMany(c => c.ComentarioConteudoOfensivo)
            .HasForeignKey(cco => cco.ConteudoOfensivoId);


    }
}