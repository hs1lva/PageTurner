
using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class AvaliacaoLivro
{
    [Key]
    public int avaliacaoId { get; set; }
    public int nota { get; set; }
    public DateTime dataAvaliacao { get; set; }

    //chave estrangeira para  utilizador
    public Utilizador utilizador { get; set; }
    //chave estrangeira para  livro
    public Livro livro { get; set; }
}