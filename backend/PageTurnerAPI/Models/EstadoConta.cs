

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

    /// <summary>
    /// Verifica se o estado da conta é válido
    /// </summary>
    /// <param name="estadoConta"></param>
    /// <returns></returns>
    public static bool EstadoContaValido(int estadoConta)
    {
        if (estadoConta == 1 || estadoConta == 2 || estadoConta == 3)
        {
            return true;
        }
        return false;
    }
}