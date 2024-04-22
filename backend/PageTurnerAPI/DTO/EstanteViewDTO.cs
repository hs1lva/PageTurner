namespace backend.Models;

public class EstanteViewDTO
{
    public int estanteId { get; set; }
    public DateTime ultimaAtualizacao { get; set; }
    //chave estrangeira para  tipoEstante
    public TipoEstante tipoEstante { get; set; }
    //chave estrangeira para  utilizador
    public Livro livro { get; set; }
    //verificar se o livro está na estante ou se foi excluído
    public bool livroNaEstante { get; set; }
    // chave estrangeira para utilizador
    public int utilizadorId { get; set; }
    public string username { get; set; }
}