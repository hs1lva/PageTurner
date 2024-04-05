namespace backend.Models;

/// <summary>
/// Data transfer object for creating or updating a book comment.
/// </summary>
public class ComentarioLivroDTO
{
    public int comentarioId { get; set; }
    public string comentario { get; set; }
    public DateTime dataComentario { get; set; }
    public int utilizadorId { get; set; }
    public int livroId { get; set; }
}