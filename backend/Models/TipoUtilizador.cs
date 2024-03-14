
using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class TipoUtilizador
{
    [Key]
    public int tipoUtilId { get; set; }
    public string descricaoTipoUti { get; set; }
}