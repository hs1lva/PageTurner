public class EstanteUpdateDTO
{
    public int estanteId { get; set; }
    public DateTime ultimaAtualizacao { get; set; }
    //chave estrangeira para  tipoEstante
    public int tipoEstanteId { get; set; }
    //chave estrangeira para  utilizador
    public int livroId { get; set; }
    //verificar se o livro está na estante ou se foi excluído
    public bool livroNaEstante { get; set; }
}