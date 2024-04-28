

using System.ComponentModel.DataAnnotations;
namespace backend.Models;
public class EstadoConta
{
    // -------- ESTADO CONTA UTILIZADOR --------
    // 1 - Ativo
    // 2 - Inativo
    // 3 - Banido
    // -----------------------------------------
    
    [Key]
    public int estadoContaId { get; set; }
    public string descricaoEstadoConta { get; set; }
}