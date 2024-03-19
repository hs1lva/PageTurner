
using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class TipoEstante
{
    [Key]
    public int tipoEstanteId { get; set; }
    [Required]
    public string descricaoTipoEstante { get; set; }
}