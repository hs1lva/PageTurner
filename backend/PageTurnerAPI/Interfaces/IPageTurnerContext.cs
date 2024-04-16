namespace backend.Interfaces
{
    using Microsoft.EntityFrameworkCore;
    using backend.Models;
    
    public interface IPageTurnerContext
    {
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
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}