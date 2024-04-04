
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;
public class EstadoTroca
{
    [Key]
    public int estadoTrocaId { get; set; }
    [Required]
    public string descricaoEstadoTroca { get; set; }

    public static async Task<EstadoTroca> ProcEstadoTroca(string estado, PageTurnerContext _bd)
    {
        if (estado == null)
        {
            throw new Exception("Estado não pode ser nulo");
        }

        //verifica se o estado existe
        var estadoTroca = await _bd.EstadoTroca
            .Where(x => x.descricaoEstadoTroca == estado)
            .FirstOrDefaultAsync();

        if (estadoTroca == null)
        {
            throw new Exception("Estado não existe");
        }
        return estadoTroca;

    }
}