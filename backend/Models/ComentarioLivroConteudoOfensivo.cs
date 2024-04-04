using System.Text.Json.Serialization;
namespace backend.Models;


public class ComentarioLivroConteudoOfensivo
{
    /// <summary>
    /// Identificador do comentário associado.
    /// </summary>
    public int ComentarioId { get; set; }
    
    /// <summary>
    /// Identificador do conteúdo ofensivo associado.
    /// </summary>
    public int ConteudoOfensivoId { get; set; }
    
    /// <summary>
    /// A propriedade de navegação para o comentário associado.
    /// Ignorada na serialização para evitar referências circulares.
    /// </summary>
    [JsonIgnore]
    public ComentarioLivro ComentarioLivro { get; set; }
    
    /// <summary>
    /// A propriedade de navegação para o conteúdo ofensivo associado.
    /// Ignorada na serialização para evitar referências circulares.
    /// </summary>
    [JsonIgnore]
    public ConteudoOfensivo ConteudoOfensivo { get; set; }
}
