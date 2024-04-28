
using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class TipoUtilizador
{
    // -------- TIPO UTILIZADOR --------
    // 1 - Administrador
    // 2 - Utilizador
    // ---------------------------------

    [Key]
    public int tipoUtilId { get; set; }
    public string descricaoTipoUti { get; set; }

    /// <summary>
    /// Verifica se o tipo de utilizador é válido
    /// </summary>
    /// <param name="tipoUtilizadorId"></param>
    /// <returns></returns>
    public static bool IsValidTipoUtilizador(int tipoUtilizadorId)
    {
        return tipoUtilizadorId == 1 || tipoUtilizadorId == 2;
    }



}