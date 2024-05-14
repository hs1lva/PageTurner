namespace backend.Models;

public class LivroCreateDTO
{
    public string TituloLivro { get; set; }
    public int AnoPrimeiraPublicacao { get; set; }
    //public string IdiomaOriginalLivro { get; set; }
    public int AutorLivroId { get; set; } 
    public int GeneroLivroId { get; set; } 
}