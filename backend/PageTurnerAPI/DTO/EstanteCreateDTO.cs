namespace backend.Models;

public class EstanteCreateDTO
{
    // chave estrangeira para tipoEstante
    public string tipoEstanteDescricao { get; set; }
    // chave estrangeira para utilizador
    public int utilizadorId { get; set; }
    // chave estrangeira para livro
    public int livroId { get; set; }
}