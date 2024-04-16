using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class GeneroLivro
{
    [Key]
    public int generoId { get; set; }
    public string descricaoGenero { get; set; }
    List<Utilizador> utilizadores { get; } = [];
}