namespace backend.Models;

public class LivroDTO
{
    public int LivroId { get; set; }
    public string TituloLivro { get; set; }
    public int AnoPrimeiraPublicacao { get; set; }
    //public string IdiomaOriginalLivro { get; set; }
    public AutorLivro AutorLivro { get; set; }
    public GeneroLivro GeneroLivro { get; set; }
    public ICollection<ComentarioLivro> Comentarios { get; set; }
    public ICollection<AvaliacaoLivro> Avaliacoes { get; set; }
    public double? MediaAvaliacao { get; set; }
    public string CapaSmall { get; set; }
    public string CapaMedium { get; set; }
    public string CapaLarge { get; set; }
}
