
using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class ConteudoOfensivo
{
    [Key]
    public int conteudoOfensivoId { get; set; }
    public string especificacaoConteudoOfensivo { get; set; }

    //chave estrangeira para lista comentario de livro
    public ICollection<ComentarioLivroConteudoOfensivo> comentarioConteudoOfensivo { get; set; }

}