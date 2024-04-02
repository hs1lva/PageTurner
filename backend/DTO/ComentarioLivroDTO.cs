namespace backend.Models;

/// <summary>
/// Data transfer object for creating or updating a book comment.
/// </summary>
public class ComentarioLivroDTO
{
    public int ComentarioId { get; set; }
    public string Comentario { get; set; }
    public DateTime DataComentario { get; set; }
    public int UtilizadorId { get; set; }
    public int LivroId { get; set; }
}