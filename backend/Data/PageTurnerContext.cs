
using backend.Models;
using Microsoft.EntityFrameworkCore;

public class PageTurnerContext:DbContext{
    public PageTurnerContext(DbContextOptions<PageTurnerContext> options) : base(options)
    {

    }

    //escrever aqui as tabelas
    public DbSet<AutorLivro> AutorLivro { get; set; }
    public DbSet<AvaliacaoLivro> AvaliacaoLivro { get; set; }
    public DbSet<Cidade> Cidade { get; set; }
    public DbSet<ComentarioLivro> CommentLivro { get; set; }
    public DbSet<ConteudoOfensivo> ConteudoOfensivo { get; set; }
    public DbSet<EstadoComentario> EstadoComentario { get; set; }
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

    
}