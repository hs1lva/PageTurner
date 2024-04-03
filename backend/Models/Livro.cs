using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class Livro
{
    [Key]
    public int livroId { get; set; }
    public string tituloLivro { get; set; }
    public int anoPrimeiraPublicacao { get; set; }
    public string idiomaOriginalLivro { get; set; }

    //chave estranjeira para  autorLivro
    public AutorLivro autorLivro { get; set; }
    //chave estranjeira para  generoLivro
    public GeneroLivro generoLivro { get; set; }
    
    // Propriedade de navegação para as avaliações
    public ICollection<AvaliacaoLivro> Avaliacoes { get; set; }
	public ICollection<ComentarioLivro> Comentarios { get; set; }

	// issue #66
	public double? MediaAvaliacao()
	{
		if (Avaliacoes == null || !Avaliacoes.Any())
		{
			return null; 
		}

		return Avaliacoes.Average(a => a.Nota);
	}
}