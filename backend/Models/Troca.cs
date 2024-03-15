
using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class Troca
{
    [Key]
    public int trocaId { get; set; }
    public DateTime dataPedidoTroca { get; set; }
    public DateTime? dataAceiteTroca { get; set; }
    public Estante estanteId2 { get; set; }
    public Estante estanteId { get; set; }
    public EstadoTroca estadoTroca { get; set; }
}