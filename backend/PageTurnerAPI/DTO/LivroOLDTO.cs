namespace backend.Models;

public class LivroOLDTO
{
    public string TituloLivro { get; set; }
    public int AnoPrimeiraPublicacao { get; set; }
    public string KeyOL { get; set; }
    public string CapaSmall { get; set; }
    public string CapaMedium { get; set; }
    public string CapaLarge { get; set; }
    public string[] AutorLivroNome { get; set; }
    public string[] GeneroLivroNome { get; set; } 
}