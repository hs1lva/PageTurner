
using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class EstadoTroca
{
    [Key]
    public int estadoTrocaId { get; set; }
    [Required]
    public string descricaoEstadoTroca { get; set; }
}