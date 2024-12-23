using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace backend.Models;
public class Livro
{
    [Key]
    public int livroId { get; set; }
    [JsonProperty("title")]
    public string tituloLivro { get; set; }
    [JsonProperty("first_publish_year")]
    public int anoPrimeiraPublicacao { get; set; }
    [JsonProperty("key")]
    public string keyOL { get; set; }
    [JsonProperty("capaSmall")]
    public string capaSmall { get; set; }
    [JsonProperty("capaMedium")]
    public string capaMedium { get; set; }
    [JsonProperty("capaLarge")]
    public string capaLarge { get; set; }
    //chave estrangeira para  autorLivro
    [JsonProperty("author_name")]
    public AutorLivro autorLivro { get; set; }
    //chave estrangeira para  generoLivro
    [JsonProperty("subject")]
    public GeneroLivro generoLivro { get; set; }

    // Propriedade de navegação para as avaliações
    public ICollection<AvaliacaoLivro> Avaliacoes { get; set; }
	public ICollection<ComentarioLivro> Comentarios { get; set; }

    // issue #66
    public double? MediaAvaliacao()
    {
        if (Avaliacoes == null || !Avaliacoes.Any())
        {
            return null;
        }

        return Avaliacoes.Average(a => a.Nota);
    }

    /// <summary>
    /// Metodo para sugerir livros com base nos livros da estante do utilizador
    /// </summary>
    /// <param name="utilizadorId"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    // Método para sugerir livros com base nos livros da estante do utilizador
    public async Task<List<LivroDTO>> SugerirLivros(int utilizadorId, PageTurnerContext context)
    {
        try
        {
            // Recuperar os livros da estante do utilizador com base no tipo da estante
            var livrosDaEstante = await context.Estante
                .Include(e => e.livro)
                    .ThenInclude(l => l.autorLivro)
                .Include(e => e.livro)
                    .ThenInclude(l => l.generoLivro)
                .Where(e => e.utilizador.utilizadorID == utilizadorId)
                .Select(e => e.livro)
                .ToListAsync();

            // Lista de Autores e Géneros dos livros da estante do utilizador
            var autoresEstante = livrosDaEstante.Select(l => l.autorLivro).ToList();
            var generosEstante = livrosDaEstante.Select(l => l.generoLivro).ToList();

            var livrosSugeridos = await context.Livro
                .Include(l => l.autorLivro)
                .Include(l => l.generoLivro)
                .Where(l => autoresEstante.Contains(l.autorLivro) || generosEstante.Contains(l.generoLivro))
                .ToListAsync();

            // Comparar as duas listas e retirar os duplicados da lista de livros sugeridos
            // Comparar duplicados dos livrosDaEstante e livrosSugeridos
            // Remover o livro que o utilizador já tem na estante
            var a = livrosSugeridos.Where(l => !livrosDaEstante.Contains(l));

            livrosSugeridos = a.ToList();

            // Mapear os livros sugeridos para LivroDTO
            var livrosSugeridosDTO = livrosSugeridos.Select(l => new LivroDTO
            {
                LivroId = l.livroId,
                TituloLivro = l.tituloLivro,
                AnoPrimeiraPublicacao = l.anoPrimeiraPublicacao,
                //IdiomaOriginalLivro = l.idiomaOriginalLivro,
                AutorLivro = l.autorLivro,
                GeneroLivro = l.generoLivro,
                MediaAvaliacao = l.MediaAvaliacao(),
                Comentarios = l.Comentarios,
                Avaliacoes = l.Avaliacoes,
                CapaSmall = l.capaSmall,
                CapaMedium = l.capaMedium,
                CapaLarge = l.capaLarge
            }).ToList();

            return livrosSugeridosDTO;
        }
        catch (Exception ex)
        {
            // Lidar com a exceção, como logá-la ou relançá-la
            throw new Exception("Erro ao sugerir livros.", ex);
        }
    }

    public async static Task<List<LivroDTO>> PesquisaLivroBd(string termo, PageTurnerContext _context)
    {
        var livros = await _context.Livro
                        .Include(l => l.autorLivro)
                        .Include(l => l.generoLivro)
                        .Where(l => l.tituloLivro.ToLower().Contains(termo.ToLower()))
                        .Select(l => new LivroDTO
                        {
                            LivroId = l.livroId,
                            TituloLivro = l.tituloLivro,
                            AutorLivro = l.autorLivro,
                            GeneroLivro = l.generoLivro
                        })
                        .ToListAsync();
                        
        return livros;
    }

    public static async Task<Livro> GetLivroByKey(string key, PageTurnerContext _context)
    {
        return await _context.Livro
            .Include(l => l.autorLivro)
            .Include(l => l.generoLivro)
            .FirstOrDefaultAsync(l => l.keyOL == key);
    }

    public static bool LivroExistsKey(string key, PageTurnerContext _context)
    {
        // Verificar se o livro já existe na base de dados
        return _context.Livro.Any(e => e.keyOL == key);
    }

    public static int LivroExistsTitulo(string titulo, PageTurnerContext _context)
    {
        // Verificar se o livro já existe na base de dados
        if (_context.Livro.Any(e => e.tituloLivro.ToLower() == titulo.ToLower()))
        {
            // Retornar o id do livro
            return _context.Livro.FirstOrDefault(e => e.tituloLivro.ToLower() == titulo.ToLower()).livroId;
        }
        return 0;
    }

}
