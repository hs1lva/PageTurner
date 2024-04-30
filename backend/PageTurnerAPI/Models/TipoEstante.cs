
using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class TipoEstante
{
    [Key]
    public int tipoEstanteId { get; set; }
    [Required]
    public string descricaoTipoEstante { get; set; }

    public static bool TipoEstanteExists(PageTurnerContext context, int id)
    {
        return context.TipoEstante.Any(e => e.tipoEstanteId == id);
    }

        public static bool TipoEstanteDescExists(PageTurnerContext context, string descricaoTipoEstante)
    {
        return context.TipoEstante.Any(e => e.descricaoTipoEstante == descricaoTipoEstante);
    }
}