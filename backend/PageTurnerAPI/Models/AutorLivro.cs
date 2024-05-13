using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class AutorLivro
{
    [Key]
    public int autorLivroId { get; set; }
    public string nomeAutor { get; set; }

    List<Utilizador> utilizadores { get; } = [];
}
