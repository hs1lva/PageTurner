
using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class ComentarioLivro
{
    [Key]
    public int comentarioId { get; set; }
    [Required]
    public string comentario { get; set; }
    public DateTime dataComentario { get; set; }

    //chave estrangeira para  utilizador
    public Utilizador utilizador { get; set; }
    //chave estrangeira para  livro
    public Livro livro { get; set; }
    //chave estrangeira para  estadoComentario
    public EstadoComentario estadoComentario { get; set; }

    //chave estrangeira para lista conteudoOfensivo
    public List<ConteudoOfensivo> conteudoOfensivo { get; } = [];

}