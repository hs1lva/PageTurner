namespace backend.Models;

public class EstanteSubmitDTO
{
    // chave estrangeira para tipoEstante
    public int tipoEstanteId { get; set; }
    // chave estrangeira para utilizador
    public int utilizadorId { get; set; }
    // chave estrangeira para livro
    public int livroId { get; set; }
}