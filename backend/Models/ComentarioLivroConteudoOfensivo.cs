using System.Text.Json.Serialization;
namespace backend.Models;


public class ComentarioLivroConteudoOfensivo
{
    /// <summary>
    /// Identificador do comentário associado.
    /// </summary>
    public int comentarioId { get; set; }
    
    /// <summary>
    /// Identificador do conteúdo ofensivo associado.
    /// </summary>
    public int conteudoOfensivoId { get; set; }
    
    /// <summary>
    /// A propriedade de navegação para o comentário associado.
    /// Ignorada na serialização para evitar referências circulares.
    /// </summary>
    [JsonIgnore]
    public ComentarioLivro comentarioLivro { get; set; }
    
    /// <summary>
    /// A propriedade de navegação para o conteúdo ofensivo associado.
    /// Ignorada na serialização para evitar referências circulares.
    /// </summary>
    [JsonIgnore]
    public ConteudoOfensivo conteudoOfensivo { get; set; }
}
