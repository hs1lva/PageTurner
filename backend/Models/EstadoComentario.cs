using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class EstadoComentario
{
    [Key]
    public int estadoComentarioId { get; set; }
    [Required]
    public string descricaoEstadoComentario { get; set; }
}