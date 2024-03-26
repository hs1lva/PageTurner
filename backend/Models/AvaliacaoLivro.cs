
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.Models;
public class AvaliacaoLivro
{
    [Key]
    public int AvaliacaoId { get; set; }

	[Range(1, 5)] 
    public int Nota { get; set; }

    public DateTime DataAvaliacao { get; set; }

    // Chaves estrangeiras
    public int UtilizadorId { get; set; }
    public int LivroId { get; set; }
}