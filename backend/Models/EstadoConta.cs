

using System.ComponentModel.DataAnnotations;
namespace backend.Models;
public class EstadoConta
{
    [Key]
    public int estadoContaId { get; set; }
    public string descricaoEstadoConta { get; set; }
}