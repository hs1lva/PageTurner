using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class Estante
{
    [Key]
    public int estanteId { get; set; }
    public DateTime ultimaAtualizacao { get; set; }
    //chave estrangeira para  tipoEstante
    public TipoEstante tipoEstante { get; set; }
    //chave estrangeira para  utilizador
    public Utilizador utilizador { get; set; }
    //chave estrangeira para livro
    public Livro livro { get; set; }
    //verificar se o livro está na estante ou se foi excluído
    public bool livroNaEstante { get; set; }
}